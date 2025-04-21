using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IAutoBidProcessingService
    {
        Task ProcessAutoBidsForAuctionAsync(int auctionId, decimal newBidAmount);
    }
} 