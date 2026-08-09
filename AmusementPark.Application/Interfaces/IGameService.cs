 
using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Interfaces
{
    public interface IGameService
    {
        List<Game> GetAll();

        Game? GetById(int id);

        List<Game> Search(string text);

        void Add(Game game);

        void Update(Game game);

        void Delete(int id);
    }
}
