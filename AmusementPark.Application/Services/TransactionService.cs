using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public List<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public Transaction? GetById(int id)
        {
            return _transactionRepository.GetById(id);
        }

        public List<Transaction> GetPlayerTransactions(int playerId)
        {
            return _transactionRepository.GetPlayerTransactions(playerId);
        }

        public void Add(Transaction transaction)
        {
            if (transaction.TotalPrice <= 0)
                throw new ArgumentException("Total price is invalid.");

            _transactionRepository.Add(transaction);
        }

        public void Update(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
        }

        public void Delete(int id)
        {
            _transactionRepository.Delete(id);
        }
    }
}