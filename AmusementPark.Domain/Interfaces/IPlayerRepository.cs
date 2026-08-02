
using AmusementPark.Domain.Entities;

namespace AmusementPark.Domain.Interfaces
{
    public class IPlayerRepository
    {
        public interface IPlayerRepositories
        {
            List<Player> GetAll();

            Player? GetById(int id);

            Player? GetByMobile(string mobile);

            void Add(Player player);

            void Update(Player player);

            void Delete(int id);
        }
    }
}
