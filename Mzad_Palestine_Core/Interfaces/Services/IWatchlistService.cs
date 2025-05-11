using Mzad_Palestine_Core.DTO_s.Watchlist;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IWatchlistService
    {
        Task<WatchlistDto> AddAsync(int userId, int listingId);
        Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(int userId);
        Task<bool> IsInWatchlistAsync(int userId, int listingId);
        Task DeleteFromWatchlistAsync(int userId, int listingId);
    }
}
