using AmusementPark.Domain.Entities;

namespace AmusementPark.Domain.Interfaces;

public interface ITicketRepository
{
    List<Ticket> GetAll();

    Ticket? GetById(int id);

    List<Ticket> GetUnusedTickets();

    List<Ticket> GetPlayerTickets(int playerId);

    void Add(Ticket ticket);

    void Update(Ticket ticket);

    void Delete(int id);
}