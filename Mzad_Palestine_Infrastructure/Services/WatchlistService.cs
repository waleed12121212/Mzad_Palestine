using Mzad_Palestine_Core.DTO_s.Watchlist;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WatchlistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<WatchlistDto> AddAsync(int userId , int listingId)
        {
            throw new NotImplementedException();
        }

        public async Task<Watchlist> AddToWatchlistAsync(CreateWatchlistDto dto)
        {
            var watchlist = new Watchlist
            {
                UserId = dto.UserId ,
                ListingId = dto.ListingId ,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.Watchlists.AddAsync(watchlist);
            await _unitOfWork.CompleteAsync();

            return watchlist;
        }

        public Task<IEnumerable<WatchlistDto>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(int userId)
        {
            return await _unitOfWork.Watchlists.FindAsync(w => w.UserId == userId);
        }

        public async Task<bool> IsInWatchlistAsync(int userId , int listingId)
        {
            var watchlist = await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId);
            return watchlist.Any();
        }

        public async Task RemoveFromWatchlistAsync(int userId , int listingId)
        {
            var watchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId)).FirstOrDefault();

            if (watchlist != null)
            {
                _unitOfWork.Watchlists.Remove(watchlist);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}