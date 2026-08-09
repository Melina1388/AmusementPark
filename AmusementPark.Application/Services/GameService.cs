using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public List<Game> GetAll()
        {
            return _gameRepository.GetAll();
        }

        public Game? GetById(int id)
        {
            return _gameRepository.GetById(id);
        }

        public List<Game> Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return _gameRepository.GetAll();

            return _gameRepository.Search(text.Trim());
        }

        public void Add(Game game)
        {
            if (string.IsNullOrWhiteSpace(game.GameName))
                throw new ArgumentException("Game name is required.");

            if (game.GamePrice <= 0)
                throw new ArgumentException("Game price is invalid.");

            _gameRepository.Add(game);
        }

        public void Update(Game game)
        {
            _gameRepository.Update(game);
        }

        public void Delete(int id)
        {
            _gameRepository.Delete(id);
        }
       
    }
}