using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmusementPark.Domain.Entities;

namespace AmusementPark.Domain.Interfaces
{
   
        public interface ITransactionRepository
        {
            List<Transaction> GetAll();

            Transaction? GetById(int id);

            List<Transaction> GetPlayerTransactions(int playerId);

        int Add(Transaction transaction);

        void Update(Transaction transaction);

            void Delete(int id);
        }

    
}
