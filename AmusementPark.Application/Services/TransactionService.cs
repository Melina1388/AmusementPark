using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{

    /// منطق مربوط به تراکنش‌ها.
    /// 
    /// این کلاس فقط با Interface مربوط به Repository
    /// کار می‌کند و از Database اطلاعی ندارد.

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(
            ITransactionRepository transactionRepository)
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


        public List<Transaction> GetPlayerTransactions(
            int playerId)
        {
            //ef

            return _transactionRepository
                .GetPlayerTransactions(playerId);
        }


  
        /// ثبت تراکنش جدید.
    
        public int Add(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(
                    nameof(transaction));
            }

            if (!transaction.PlayerID.HasValue)
            {
                throw new ArgumentException(
                    "Player ID is required.");
            }

            if (!transaction.TotalPrice.HasValue ||
                transaction.TotalPrice <= 0)
            {
                throw new ArgumentException(
                    "Transaction amount must be greater than zero.");
            }

            return _transactionRepository.Add(
                transaction);
        }


        public void Update(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(
                    nameof(transaction));
            }

            _transactionRepository.Update(
                transaction);
        }


        public void Delete(int id)
        {
            _transactionRepository.Delete(id);
        }
    }
}