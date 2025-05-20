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

        public async Task<IEnumerable<WatchlistDto>> GetByUserIdAsync(int userId)
        {
            var watchlists = await _unitOfWork.Watchlists.FindAsync(w => w.UserId == userId);
            return watchlists.Select(w => new WatchlistDto
            {
                Id = w.WatchlistId,
                UserId = w.UserId,
                ListingId = w.ListingId,
                AuctionId = w.AuctionId,
                AddedAt = w.AddedAt
            });
        }

        public async Task<WatchlistDto> AddListingAsync(int userId, int listingId)
        {
            // Check if the item is already in the watchlist
            var existingWatchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId)).FirstOrDefault();

            if (existingWatchlist != null)
            {
                throw new Exception("هذه القائمة موجودة بالفعل في المفضلة");
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

        public async Task<WatchlistDto> AddAuctionAsync(int userId, int auctionId)
        {
            // Check if the item is already in the watchlist
            var existingWatchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.AuctionId == auctionId)).FirstOrDefault();

            if (existingWatchlist != null)
            {
                throw new Exception("هذا المزاد موجود بالفعل في المفضلة");
            }

            var watchlist = new Watchlist
            {
                UserId = userId,
                AuctionId = auctionId,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.Watchlists.AddAsync(watchlist);
            await _unitOfWork.CompleteAsync();

            return new WatchlistDto
            {
                Id = watchlist.WatchlistId,
                UserId = watchlist.UserId,
                AuctionId = watchlist.AuctionId,
                AddedAt = watchlist.AddedAt
            };
        }

        public async Task RemoveListingAsync(int userId, int listingId)
        {
            var watchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId)).FirstOrDefault();

            if (watchlist == null)
            {
                throw new Exception("هذه القائمة غير موجودة في المفضلة");
            }

            await _unitOfWork.Watchlists.DeleteAsync(watchlist);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveAuctionAsync(int userId, int auctionId)
        {
            var watchlist = (await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.AuctionId == auctionId)).FirstOrDefault();

            if (watchlist == null)
            {
                throw new Exception("هذا المزاد غير موجود في المفضلة");
            }

            await _unitOfWork.Watchlists.DeleteAsync(watchlist);
            await _unitOfWork.CompleteAsync();
        }
    }
}