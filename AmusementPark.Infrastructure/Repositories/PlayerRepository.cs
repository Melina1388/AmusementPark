using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;

namespace AmusementPark.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    public List<Player> GetAll()
    {
        List<Player> players = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM Player";

        using OleDbCommand command = new(query, connection);

        using OleDbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            players.Add(new Player
            {
                PlayerID = Convert.ToInt32(reader["PlayerID"]),
                PlayerName = reader["PlayerName"].ToString()!,
                PlayerMobile = reader["PlayerMobile"].ToString()!
            });
        }

        return players;
    }

    public Player? GetById(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM Player WHERE PlayerID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerID", id);

        using OleDbDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Player
            {
                PlayerID = Convert.ToInt32(reader["PlayerID"]),
                PlayerName = reader["PlayerName"].ToString()!,
                PlayerMobile = reader["PlayerMobile"].ToString()!
            };
        }

        return null;
    }

    public Player? GetByMobile(string mobile)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM Player WHERE PlayerMobile=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerMobile", mobile);

        using OleDbDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Player
            {
                PlayerID = Convert.ToInt32(reader["PlayerID"]),
                PlayerName = reader["PlayerName"].ToString()!,
                PlayerMobile = reader["PlayerMobile"].ToString()!
            };
        }

        return null;
    }

    public void Add(Player player)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query =
            "INSERT INTO Player (PlayerName,PlayerMobile) VALUES (?,?)";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerName", player.PlayerName);
        command.Parameters.AddWithValue("@PlayerMobile", player.PlayerMobile);

        command.ExecuteNonQuery();
    }

    public void Update(Player player)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query =
            "UPDATE Player SET PlayerName=?, PlayerMobile=? WHERE PlayerID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerName", player.PlayerName);
        command.Parameters.AddWithValue("@PlayerMobile", player.PlayerMobile);
        command.Parameters.AddWithValue("@PlayerID", player.PlayerID);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "DELETE FROM Player WHERE PlayerID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerID", id);

        command.ExecuteNonQuery();
    }
}