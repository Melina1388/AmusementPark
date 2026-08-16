using AmusementPark.Domain.Entities;

namespace AmusementPark.Application.Interfaces
{
  
    /// سرویس مربوط به عملیات تراکنش‌های مالی.
    /// این Interface در لایه Application قرار دارد
    /// و هیچ وابستگی به Database یا MVC ندارد.
   
    public interface ITransactionService
    {
        List<Transaction> GetAll();

        Transaction? GetById(int id);

        List<Transaction> GetPlayerTransactions(int playerId);

        int Add(Transaction transaction);

        void Update(Transaction transaction);

        void Delete(int id);
    }
}