using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public List<Ticket> GetAll()
        {
            return _ticketRepository.GetAll();
        }

        public Ticket? GetById(int id)
        {
            return _ticketRepository.GetById(id);
        }

        public List<Ticket> GetUnusedTickets()
        {
            return _ticketRepository.GetUnusedTickets();
        }

        public void Add(Ticket ticket)
        {
            _ticketRepository.Add(ticket);
        }

        public void Update(Ticket ticket)
        {
            _ticketRepository.Update(ticket);
        }

        public void Delete(int id)
        {
            _ticketRepository.Delete(id);
        }
    }
}