using Microsoft.AspNetCore.Mvc;
using AmusementPark.Application.Interfaces;

namespace AmusementPark.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameApiController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameApiController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var games = _gameService.GetAll();

            return Ok(games);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var game = _gameService.GetById(id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }
        [HttpGet("search")]
        public IActionResult Search(string text)
        {
            var games = _gameService.Search(text);

            return Ok(games);
        }
    }
}