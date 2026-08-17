using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;

namespace AmusementPark.Infrastructure.Repositories;

public class PaymentRepository : ITransactionRepository
{
    public List<Transaction> GetAll()
    {
        List<Transaction> transactions = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM [Transaction]";

        using OleDbCommand command = new(query, connection);

        using OleDbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            transactions.Add(new Transaction
            {
                TransactionID = Convert.ToInt32(reader["TransactionID"]),
                PlayerID = Convert.ToInt32(reader["PlayerID"]),
                CardNum = reader["CardNum"].ToString()!,
                TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                TrackingNum = reader["TrackingNum"].ToString()!
            });
        }

        return transactions;
    }

    public Transaction? GetById(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM [Transaction] WHERE TransactionID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@TransactionID", id);

        using OleDbDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Transaction
            {
                TransactionID = Convert.ToInt32(reader["TransactionID"]),
                PlayerID = Convert.ToInt32(reader["PlayerID"]),
                CardNum = reader["CardNum"].ToString()!,
                TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                TrackingNum = reader["TrackingNum"].ToString()!
            };
        }

        return null;
    }

    public List<Transaction> GetPlayerTransactions(int playerId)
    {
        List<Transaction> transactions = new();

        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "SELECT * FROM [Transaction] WHERE PlayerID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerID", playerId);

        using OleDbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            transactions.Add(new Transaction
            {
                TransactionID = Convert.ToInt32(reader["TransactionID"]),
                PlayerID = Convert.ToInt32(reader["PlayerID"]),
                CardNum = reader["CardNum"].ToString()!,
                TotalPrice = Convert.ToDecimal(reader["TotalPrice"]),
                TrackingNum = reader["TrackingNum"].ToString()!
            });
        }

        return transactions;
    }

    public int Add(Transaction transaction)
    {
        using OleDbConnection connection =
            AccessConnection.GetConnection();

        connection.Open();


        string query =
            @"INSERT INTO [Transaction]
        (PlayerID, CardNum, TotalPrice, TrackingNum)
        VALUES (?,?,?,?)";


        using OleDbCommand command =
            new(query, connection);

        
        command.Parameters.AddWithValue(
            "@PlayerID",
            transaction.PlayerID);

        command.Parameters.AddWithValue(
            "@CardNum",
            transaction.CardNum);
        

        command.Parameters.AddWithValue(
            "@TotalPrice",
            transaction.TotalPrice);

        command.Parameters.AddWithValue(
            "@TrackingNum",
            transaction.TrackingNum);
      

        command.ExecuteNonQuery();


        // دریافت ID رکورد تازه ایجاد شده

        command.CommandText =
            "SELECT @@IDENTITY";

        command.Parameters.Clear();


        object result =
            command.ExecuteScalar();


        int transactionId =
            Convert.ToInt32(result);


        transaction.TransactionID =
            transactionId;


        return transactionId;
    }
    public void Update(Transaction transaction)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query =
            @"UPDATE [Transaction]
              SET PlayerID=?,
                  CardNum=?,
                  TotalPrice=?,
                  TrackingNum=?
              WHERE TransactionID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@PlayerID", transaction.PlayerID);
        command.Parameters.AddWithValue("@CardNum", transaction.CardNum);
        command.Parameters.AddWithValue("@TotalPrice", transaction.TotalPrice);
        command.Parameters.AddWithValue("@TrackingNum", transaction.TrackingNum);
        command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using OleDbConnection connection = AccessConnection.GetConnection();

        connection.Open();

        string query = "DELETE FROM [Transaction] WHERE TransactionID=?";

        using OleDbCommand command = new(query, connection);

        command.Parameters.AddWithValue("@TransactionID", id);

        command.ExecuteNonQuery();
    }
}