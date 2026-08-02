 
using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Interfaces
{
    public interface IGameService
    {
        List<Game> GetAllGames();

        Game? GetGame(int id);

        List<Game> SearchGames(string keyword);

        void Create(Game game);

        void Edit(Game game);


        void Remove(int id);
    }
}
