using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(
            IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // ==========================================
        // Games
        // ==========================================

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
                throw new ArgumentException(
                    "Game name is required.");

            if (game.GamePrice == null ||
                game.GamePrice <= 0)
            {
                throw new ArgumentException(
                    "Game price is invalid.");
            }

            if (string.IsNullOrWhiteSpace(
                game.AmusementName))
            {
                throw new ArgumentException(
                    "Amusement park name is required.");
            }

            game.GameName =
                game.GameName.Trim();

            game.AmusementName =
                game.AmusementName.Trim();

            _gameRepository.Add(game);
        }

        public void Update(Game game)
        {
            if (game.GameID == null)
                throw new ArgumentException(
                    "Game ID is required.");

            if (string.IsNullOrWhiteSpace(
                game.GameName))
            {
                throw new ArgumentException(
                    "Game name is required.");
            }

            if (game.GamePrice == null ||
                game.GamePrice <= 0)
            {
                throw new ArgumentException(
                    "Game price is invalid.");
            }

            if (string.IsNullOrWhiteSpace(
                game.AmusementName))
            {
                throw new ArgumentException(
                    "Amusement park name is required.");
            }

            _gameRepository.Update(game);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Invalid game ID.");

            _gameRepository.Delete(id);
        }

        // ==========================================
        // Amusement Parks
        // ==========================================

        public List<string> GetAmusementParks()
        {
            return _gameRepository
                .GetAmusementParks();
        }

        public void AddAmusementPark(
            string amusementName)
        {
            if (string.IsNullOrWhiteSpace(
                amusementName))
            {
                throw new ArgumentException(
                    "Amusement park name is required.");
            }

            amusementName =
                amusementName.Trim();

            if (_gameRepository
                .AmusementParkExists(amusementName))
            {
                throw new InvalidOperationException(
                    "This amusement park already exists.");
            }

            /*
             * چون در طراحی فعلی فقط Game داریم،
             * شهربازی بدون Game رکورد مستقلی ندارد.
             *
             * بنابراین AddAmusementPark به تنهایی
             * رکوردی در Game ایجاد نمی‌کند.
             *
             * شهربازی زمانی وارد دیتابیس می‌شود
             * که اولین Game آن ثبت شود.
             */
        }

        public void RenameAmusementPark(
            string oldName,
            string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName))
                throw new ArgumentException(
                    "Old amusement park name is required.");

            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException(
                    "New amusement park name is required.");

            oldName = oldName.Trim();
            newName = newName.Trim();

            if (oldName.Equals(
                newName,
                StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (_gameRepository
                .AmusementParkExists(newName))
            {
                throw new InvalidOperationException(
                    "Another amusement park with this name already exists.");
            }

            if (!_gameRepository
                .AmusementParkExists(oldName))
            {
                throw new KeyNotFoundException(
                    "Amusement park not found.");
            }

            _gameRepository.RenameAmusementPark(
                oldName,
                newName);
        }

        public void DeleteAmusementPark(
            string amusementName)
        {
            if (string.IsNullOrWhiteSpace(
                amusementName))
            {
                throw new ArgumentException(
                    "Amusement park name is required.");
            }

            amusementName =
                amusementName.Trim();

            if (!_gameRepository
                .AmusementParkExists(amusementName))
            {
                throw new KeyNotFoundException(
                    "Amusement park not found.");
            }

            /*
             * چون Game.AmusementName همان دسته‌بندی است،
             * حذف شهربازی یعنی حذف تمام Gameهای آن.
             *
             * این رفتار را عمداً مشخص کرده‌ایم تا
             * Gameهایی با دسته‌بندی حذف‌شده باقی نمانند.
             */
            _gameRepository.DeleteAmusementPark(
                amusementName);
        }
    }
}