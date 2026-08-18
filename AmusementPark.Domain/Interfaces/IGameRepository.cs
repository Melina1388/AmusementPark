using AmusementPark.Domain.Entities;

namespace AmusementPark.Domain.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetAll();

        Game? GetById(int id);

        List<Game> Search(string keyword);

        void Add(Game game);

        void Update(Game game);

        void Delete(int id);

        // ==============================
        // Amusement Park Management
        // ==============================

        List<string> GetAmusementParks();

        bool AmusementParkExists(string amusementName);

        void RenameAmusementPark(
            string oldName,
            string newName);

        void DeleteAmusementPark(
            string amusementName);
    }
}