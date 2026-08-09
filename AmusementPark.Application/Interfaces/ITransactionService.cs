using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Services;

public interface ITransactionService
{
    List<Transaction> GetAll();

    Transaction? GetById(int id);

    List<Transaction> GetPlayerTransactions(int playerId);

    void Add(Transaction transaction);

    void Update(Transaction transaction);

    void Delete(int id);
}