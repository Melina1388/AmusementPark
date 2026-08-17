using System.Security.Claims;
using AmusementPark.Application.Interfaces;
using AmusementPark.wb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.wb.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IGameService _gameService;
        private readonly ITransactionService _transactionService;

        public TicketController(
            ITicketService ticketService,
            IGameService gameService,
            ITransactionService transactionService)
        {
            _ticketService = ticketService;
            _gameService = gameService;
            _transactionService = transactionService;
        }

        public IActionResult Ticket()
        {



            string? playerIdValue =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!int.TryParse(
                    playerIdValue,
                    out int playerId))
            {
                return RedirectToAction(
                    "Login",
                    "Login");
            }

            var tickets =
                _ticketService.GetPlayerTickets(
                    playerId);

            List<TicketViewModel> model = new();

            foreach (var ticket in tickets)
            {
                if (!ticket.GameID.HasValue ||
                    !ticket.TransactionID.HasValue)
                {
                    continue;
                }

                var game =
                    _gameService.GetById(
                        ticket.GameID.Value);

                var transaction =
                    _transactionService.GetById(
                        ticket.TransactionID.Value);

                if (game == null ||
                    transaction == null)
                {
                    continue;
                }

                model.Add(new TicketViewModel
                {
                    TicketID =
                        ticket.TicketID ?? 0,

                    GameID =
                        ticket.GameID.Value,

                    TransactionID =
                        ticket.TransactionID.Value,

                    GameName =
                        game.GameName ?? "",

                    AmusementName =
                        game.AmusementName ?? "",

                    GamePrice =
                        game.GamePrice ?? 0,

                    TrackingNum =
                        transaction.TrackingNum ?? "",

                    IsUsed =
                        ticket.IsUsed ?? ""
                });
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateStatus(int ticketId, string status)
        {
            var ticket = _ticketService.GetById(ticketId);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.IsUsed = status;

            _ticketService.Update(ticket);

            return Ok(new
            {
                success = true,
                ticketId = ticketId,
                status = status
            });
        }
    }
}