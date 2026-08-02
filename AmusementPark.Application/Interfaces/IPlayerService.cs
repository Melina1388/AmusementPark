using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Services;

public interface IPlayerService
{
    List<Player> GetAllPlayers();

    Player? GetPlayer(int id);

    Player? FindByMobile(string mobile);

    void Register(Player player);

    void Update(Player player);

    void Delete(int id);
}