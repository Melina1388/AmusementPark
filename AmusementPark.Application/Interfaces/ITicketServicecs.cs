using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Services;

public interface ITicketService
{
    List<Ticket> GetAll();

    Ticket? GetById(int id);

    List<Ticket> GetUnusedTickets();

    void Add(Ticket ticket);

    void Update(Ticket ticket);

    void Delete(int id);
}