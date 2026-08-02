using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Services;

public interface ITicketService
{
    List<Ticket> GetAllTickets();

    Ticket? GetTicket(int id);

    List<Ticket> GetAvailableTickets();

    void Create(Ticket ticket);

    void Update(Ticket ticket);

    void Delete(int id);
}