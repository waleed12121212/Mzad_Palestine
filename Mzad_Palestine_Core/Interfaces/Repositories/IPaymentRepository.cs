using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces;

namespace Mzad_Palestine_Core.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetByAuctionIdAsync(int auctionId);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId);
        Task<Payment> GetPaymentByAuctionAsync(int auctionId);
    }
} 