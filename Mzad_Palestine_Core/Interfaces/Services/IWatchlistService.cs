using Mzad_Palestine_Core.DTO_s.Watchlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistDto>> GetByUserIdAsync(int userId);
        Task<WatchlistDto> AddListingAsync(int userId, int listingId);
        Task<WatchlistDto> AddAuctionAsync(int userId, int auctionId);
        Task RemoveListingAsync(int userId, int listingId);
        Task RemoveAuctionAsync(int userId, int auctionId);
    }
}
