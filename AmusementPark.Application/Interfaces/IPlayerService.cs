using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Interfaces
{
    /// <summary>
    /// قرارداد منطق مربوط به Player.
    ///
    /// Controller مستقیماً با Repository کار نمی‌کند.
    /// Controller → Service → Repository
    /// </summary>
    public interface IPlayerService
    {
        List<Player> GetAll();

        Player? GetById(int id);

        Player? GetByMobile(string mobile);

        Player? GetByName(string playerName);

        Player? GetByNameAndMobile(
            string playerName,
            string mobile);

        void Add(Player player);

        void Update(Player player);

        void Delete(int id);
    }
}