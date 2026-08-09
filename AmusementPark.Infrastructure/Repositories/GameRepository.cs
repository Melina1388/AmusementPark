using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;


namespace AmusementPark.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private static Game MapGame(OleDbDataReader reader)
    {
        return new Game
        {
            GameID = Convert.ToInt32(reader["GameID"]),
            GameName = reader["GameName"]?.ToString() ?? "",
            AmusementName = reader["AmusementName"]?.ToString() ?? "",
            GamePrice = Convert.ToDecimal(reader["GamePrice"]),
            GameComment = reader["GameComment"]?.ToString() ?? "",
            GamePic = reader["GamePic"]?.ToString() ?? ""
        };
    }
    public List<Game> GetAll()
    {
        List<Game> games = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();


        string sql = "SELECT * FROM Game";


        using OleDbCommand command = new(sql, connection);


        using OleDbDataReader reader = command.ExecuteReader();


        while (reader.Read())
        {
            games.Add(MapGame(reader));
        }


        return games;
    }

    public Game? GetById(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string sql = "SELECT * FROM Game WHERE GameID = ?";
        using OleDbCommand command = new(sql, connection);

        command.Parameters.AddWithValue("@GameID", id);

        using OleDbDataReader reader = command.ExecuteReader();

       if (reader.Read())
{
    return MapGame(reader);
}

        return null;
    }

    public List<Game> Search(string text)
    {
        List<Game> games = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = @"
        SELECT *
        FROM Game
        WHERE AmusementName LIKE ?
           OR GameName LIKE ?
        ORDER BY AmusementName";

        using OleDbCommand command = new(query, connection);

        string searchText = "%" + text.Trim() + "%";

        // در OleDb ترتیب پارامترها مهم است.
        command.Parameters.AddWithValue("@AmusementName", searchText);
        command.Parameters.AddWithValue("@GameName", searchText);

        using OleDbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            games.Add(MapGame(reader));
        }

        return games;
    }


    public void Add(Game game)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string sql = @"INSERT INTO Game
                   (
                        GameName,
                        AmusementName,
                        GamePrice,
                        GameComment,
                        GamePic
                   )
                   VALUES
                   (
                        ?, ?, ?, ?, ?
                   )";

        using OleDbCommand command = new(sql, connection);

        command.Parameters.AddWithValue("@GameName", game.GameName);
        command.Parameters.AddWithValue("@AmusementName", game.AmusementName);
        command.Parameters.AddWithValue("@GamePrice", game.GamePrice);
        command.Parameters.AddWithValue("@GameComment", game.GameComment);
        command.Parameters.AddWithValue("@GamePic", game.GamePic);

        command.ExecuteNonQuery();
    }

    public void Update(Game game)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string sql = @"UPDATE Game
                   SET

                        GameName=? ,
                        AmusementName=? ,
                        GamePrice=? ,
                        GameComment=? ,
                        GamePic=?

                   WHERE GameID=?";

        using OleDbCommand command = new(sql, connection);

        command.Parameters.AddWithValue("@GameName", game.GameName);
        command.Parameters.AddWithValue("@AmusementName", game.AmusementName);
        command.Parameters.AddWithValue("@GamePrice", game.GamePrice);
        command.Parameters.AddWithValue("@GameComment", game.GameComment);
        command.Parameters.AddWithValue("@GamePic", game.GamePic);
        command.Parameters.AddWithValue("@GameID", game.GameID);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string sql = "DELETE FROM Game WHERE GameID=?";

        using OleDbCommand command = new(sql, connection);

        command.Parameters.AddWithValue("@GameID", id);

        command.ExecuteNonQuery();
    }
}