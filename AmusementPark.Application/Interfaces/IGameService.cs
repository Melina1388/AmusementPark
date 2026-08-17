using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Interfaces
{
    public interface IGameService
    {
        // ==============================
        // Games
        // ==============================

        List<Game> GetAll();

        Game? GetById(int id);

        List<Game> Search(string text);

        void Add(Game game);

        void Update(Game game);

        void Delete(int id);


        // ==============================
        // Amusement Parks
        // ==============================

        List<string> GetAmusementParks();

        void AddAmusementPark(string amusementName);

        void RenameAmusementPark(
            string oldName,
            string newName);

        void DeleteAmusementPark(
            string amusementName);
    }
}