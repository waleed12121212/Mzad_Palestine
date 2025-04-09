using Mzad_Palestine_Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId);
    }
}
