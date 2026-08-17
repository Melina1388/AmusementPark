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

        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query = @"
            SELECT
                TicketID,
                PlayerID,
                GameID,
                TransactionID,
                IsUsed
            FROM Ticket";

        using OleDbCommand command =
            new(query, connection);

        using OleDbDataReader reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            tickets.Add(new Ticket
            {
                TicketID = reader["TicketID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TicketID"]),

                PlayerID = reader["PlayerID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["PlayerID"]),

                GameID = reader["GameID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["GameID"]),

                TransactionID = reader["TransactionID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TransactionID"]),

                IsUsed = reader["IsUsed"] == DBNull.Value
                    ? null
                    : reader["IsUsed"].ToString()
            });
        }

        return tickets;
    }

    public Ticket? GetById(int id)
    {
        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query = @"
            SELECT
                TicketID,
                PlayerID,
                GameID,
                TransactionID,
                IsUsed
            FROM Ticket
            WHERE TicketID=?";

        using OleDbCommand command =
            new(query, connection);

        command.Parameters.AddWithValue(
            "@TicketID",
            id);

        using OleDbDataReader reader =
            command.ExecuteReader();

        if (reader.Read())
        {
            return new Ticket
            {
                TicketID = reader["TicketID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TicketID"]),

                PlayerID = reader["PlayerID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["PlayerID"]),

                GameID = reader["GameID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["GameID"]),

                TransactionID = reader["TransactionID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TransactionID"]),

                IsUsed = reader["IsUsed"] == DBNull.Value
                    ? null
                    : reader["IsUsed"].ToString()
            };
        }

        return null;
    }

    public List<Ticket> GetUnusedTickets()
    {
        List<Ticket> tickets = new();

        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query = @"
            SELECT
                TicketID,
                PlayerID,
                GameID,
                TransactionID,
                IsUsed
            FROM Ticket
            WHERE IsUsed=?";

        using OleDbCommand command =
            new(query, connection);

        // فعلاً مقدار مورد استفاده برای بلیط استفاده‌نشده
        command.Parameters.AddWithValue(
            "@IsUsed",
            "Unused");

        using OleDbDataReader reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            tickets.Add(new Ticket
            {
                TicketID = reader["TicketID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TicketID"]),

                PlayerID = reader["PlayerID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["PlayerID"]),

                GameID = reader["GameID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["GameID"]),

                TransactionID = reader["TransactionID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TransactionID"]),

                IsUsed = reader["IsUsed"] == DBNull.Value
                    ? null
                    : reader["IsUsed"].ToString()
            });
        }

        return tickets;
    }

    public void Add(Ticket ticket)
    {
        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query = @"
            INSERT INTO Ticket
            (
                PlayerID,
                GameID,
                TransactionID,
                IsUsed
            )
            VALUES (?,?,?,?)";

        using OleDbCommand command =
            new(query, connection);

        command.Parameters.AddWithValue(
            "@PlayerID",
            ticket.PlayerID);

        command.Parameters.AddWithValue(
            "@GameID",
            ticket.GameID);

        command.Parameters.AddWithValue(
            "@TransactionID",
            ticket.TransactionID);

        command.Parameters.AddWithValue(
            "@IsUsed",
            ticket.IsUsed);

        command.ExecuteNonQuery();
    }

    public void Update(Ticket ticket)
    {
        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query = @"
            UPDATE Ticket
            SET
                IsUsed=?
            WHERE TicketID=?";

        using OleDbCommand command =
            new(query, connection);

        command.Parameters.AddWithValue(
            "@IsUsed",
            ticket.IsUsed);

        command.Parameters.AddWithValue(
            "@TicketID",
            ticket.TicketID);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query =
            "DELETE FROM Ticket WHERE TicketID=?";

        using OleDbCommand command =
            new(query, connection);

        command.Parameters.AddWithValue(
            "@TicketID",
            id);

        command.ExecuteNonQuery();
    }

    public List<Ticket> GetPlayerTickets(int playerId)
    {
        List<Ticket> tickets = new();

        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();

        string query = @"
            SELECT
                TicketID,
                PlayerID,
                GameID,
                TransactionID,
                IsUsed
            FROM Ticket
            WHERE PlayerID=?";

        using OleDbCommand command =
            new(query, connection);

        command.Parameters.AddWithValue(
            "@PlayerID",
            playerId);

        using OleDbDataReader reader =
            command.ExecuteReader();

        while (reader.Read())
        {
            tickets.Add(new Ticket
            {
                TicketID = reader["TicketID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TicketID"]),

                PlayerID = reader["PlayerID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["PlayerID"]),

                GameID = reader["GameID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["GameID"]),

                TransactionID = reader["TransactionID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["TransactionID"]),

                IsUsed = reader["IsUsed"] == DBNull.Value
                    ? null
                    : reader["IsUsed"].ToString()
            });
        }

        return tickets;
    }
}