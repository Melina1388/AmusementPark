using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Services;

public interface ITransactionService
{
    List<Transaction> GetAllTransactions();

    Transaction? GetTransaction(int id);

    List<Transaction> GetPlayerTransactions(int playerId);

    void Create(Transaction transaction);

    void Delete(int id);
}