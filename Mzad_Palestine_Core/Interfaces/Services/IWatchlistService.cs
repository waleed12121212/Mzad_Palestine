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
        Task<WatchlistDto> AddAsync(int userId , int listingId);
        Task<IEnumerable<WatchlistDto>> GetByUserIdAsync(int userId);
        Task DeleteFromWatchlistAsync(int userId, int listingId);
    }
}
