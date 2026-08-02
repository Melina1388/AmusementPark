
using AmusementPark.Domain.Entities;
namespace AmusementPark.Domain.Interfaces
{
    public class IGameRepository
    {
        public interface IGameRepositories
        {
            List<Game> GetAll();

            Game? GetById(int id);

            List<Game> Search(string keyword);

            void Add(Game game);

            void Update(Game game);

            void Delete(int id);

        }
    }
}
