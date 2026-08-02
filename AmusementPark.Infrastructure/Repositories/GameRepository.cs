using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;

namespace AmusementPark.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
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
            games.Add(new Game
            {
                GameID = Convert.ToInt32(reader["GameID"]),
                GameName = reader["GameName"].ToString()!,
                AmusementName = reader["AmusementName"].ToString()!,
                GamePrice = reader["GamePrice"].ToString()!,
                GameComment = reader["GameComment"].ToString()!,
                GamePic = reader["GamePic"].ToString()!
            });
        }

        return games;
    }

    public Game? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Game> Search(string keyword)
    {
        throw new NotImplementedException();
    }

    public void Add(Game game)
    {
        throw new NotImplementedException();
    }

    public void Update(Game game)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}