using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Interfaces;

public interface ITicketService
{
    List<Ticket> GetAll();

    Ticket? GetById(int id);

    List<Ticket> GetUnusedTickets();

    List<Ticket> GetPlayerTickets(int playerId);

    void Add(Ticket ticket);

    void Update(Ticket ticket);

    void Delete(int id);
}