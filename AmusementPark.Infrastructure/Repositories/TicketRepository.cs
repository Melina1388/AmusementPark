using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;

namespace AmusementPark.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    public List<Ticket> GetAll()
    {
        List<Ticket> tickets = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM Ticket";

        using OleDbCommand command = new(query, connection);

        using OleDbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            tickets.Add(new Ticket
            {
                TicketID = Convert.ToInt32(reader["TicketID"]),
                IsUsed = Convert.ToBoolean(reader["IsUsed"])
            });
        }

        return tickets;
    }

    public Ticket? GetById(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM Ticket WHERE TicketID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@TicketID", id);

        using OleDbDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Ticket
            {
                TicketID = Convert.ToInt32(reader["TicketID"]),
                IsUsed = Convert.ToBoolean(reader["IsUsed"])
            };
        }

        return null;
    }

    public List<Ticket> GetUnusedTickets()
    {
        List<Ticket> tickets = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM Ticket WHERE IsUsed=False";

        using OleDbCommand command = new(query, connection);

        using OleDbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            tickets.Add(new Ticket
            {
                TicketID = Convert.ToInt32(reader["TicketID"]),
                IsUsed = Convert.ToBoolean(reader["IsUsed"])
            });
        }

        return tickets;
    }

    public void Add(Ticket ticket)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "INSERT INTO Ticket (IsUsed) VALUES (?)";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@IsUsed", ticket.IsUsed);

        command.ExecuteNonQuery();
    }

    public void Update(Ticket ticket)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "UPDATE Ticket SET IsUsed=? WHERE TicketID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@IsUsed", ticket.IsUsed);
        command.Parameters.AddWithValue("@TicketID", ticket.TicketID);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "DELETE FROM Ticket WHERE TicketID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@TicketID", id);

        command.ExecuteNonQuery();
    }
}