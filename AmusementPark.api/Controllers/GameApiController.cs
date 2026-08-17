using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AmusementPark.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameApiController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameApiController(
            IGameService gameService)
        {
            _gameService = gameService;
        }


        // =====================================================
        // Games
        // =====================================================

        [HttpGet]
        public IActionResult GetAll()
        {
            var games =
                _gameService.GetAll();

            return Ok(games);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var game =
                _gameService.GetById(id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }


        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string text)
        {
            var games =
                _gameService.Search(text);

            return Ok(games);
        }


        [HttpPost]
        public IActionResult Add(
            [FromBody] Game game)
        {
            if (game == null)
            {
                return BadRequest(
                    "اطلاعات بازی ارسال نشده است.");
            }

            try
            {
                _gameService.Add(game);

                return Ok(game);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    });
            }
        }


        [HttpPut("{id:int}")]
        public IActionResult Update(
            int id,
            [FromBody] Game game)
        {
            if (game == null)
            {
                return BadRequest(
                    "اطلاعات بازی ارسال نشده است.");
            }

            try
            {
                game.GameID = id;

                _gameService.Update(game);

                return Ok(game);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    });
            }
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _gameService.Delete(id);

                return Ok(new
                {
                    message =
                        "بازی با موفقیت حذف شد."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    });
            }
        }


        // =====================================================
        // Amusement Parks
        // =====================================================

        [HttpGet("parks")]
        public IActionResult GetAmusementParks()
        {
            var parks =
                _gameService.GetAmusementParks();

            return Ok(parks);
        }


        [HttpPut("parks")]
        public IActionResult RenameAmusementPark(
            [FromBody] RenameAmusementParkRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                    "اطلاعات شهربازی ارسال نشده است.");
            }

            try
            {
                _gameService.RenameAmusementPark(
                    request.OldName,
                    request.NewName);

                return Ok(new
                {
                    message =
                        "نام شهربازی با موفقیت تغییر کرد."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(
                    new
                    {
                        message = ex.Message
                    });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(
                    new
                    {
                        message = ex.Message
                    });
            }
        }


        [HttpDelete("parks")]
        public IActionResult DeleteAmusementPark(
            [FromQuery] string name)
        {
            try
            {
                _gameService.DeleteAmusementPark(
                    name);

                return Ok(new
                {
                    message =
                        "شهربازی با موفقیت حذف شد."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(
                    new
                    {
                        message = ex.Message
                    });
            }
        }
    }


    // =========================================================
    // Request Model
    // =========================================================

    public class RenameAmusementParkRequest
    {
        public string OldName { get; set; } = "";

        public string NewName { get; set; } = "";
    }
}