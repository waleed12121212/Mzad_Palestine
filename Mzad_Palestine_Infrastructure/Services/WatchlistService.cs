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

        public async Task<WatchlistDto> AddAsync(int userId, int listingId)
        {
            // Check if the item is already in the watchlist
            var existingWatchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId)).FirstOrDefault();

            if (existingWatchlist != null)
            {
                throw new Exception("هذا المنتج موجود بالفعل في المفضلة");
            }

            var watchlist = new Watchlist
            {
                UserId = userId,
                ListingId = listingId,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.Watchlists.AddAsync(watchlist);
            await _unitOfWork.CompleteAsync();

            return new WatchlistDto
            {
                Id = watchlist.WatchlistId,
                UserId = watchlist.UserId,
                ListingId = watchlist.ListingId,
                AddedAt = watchlist.AddedAt
            };
        }

        public async Task<IEnumerable<WatchlistDto>> GetByUserIdAsync(int userId)
        {
            var watchlists = await _unitOfWork.Watchlists.FindAsync(w => w.UserId == userId);
            return watchlists.Select(w => new WatchlistDto
            {
                Id = w.WatchlistId,
                UserId = w.UserId,
                ListingId = w.ListingId,
                AddedAt = w.AddedAt
            });
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

        public async Task DeleteFromWatchlistAsync(int userId, int listingId)
        {
            var watchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId)).FirstOrDefault();

            if (watchlist != null)
            {
                await _unitOfWork.Watchlists.DeleteAsync(watchlist);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}