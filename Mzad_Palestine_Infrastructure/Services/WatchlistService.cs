using AutoMapper;
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
        private readonly IMapper _mapper;

        public WatchlistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WatchlistDto> AddAsync(int userId, int listingId)
        {
            // التحقق من وجود المنتج في قائمة المتابعة
            var existingWatchlist = await _unitOfWork.Watchlists.FindAsync(w =>
                w.UserId == userId && w.ListingId == listingId);

            if (existingWatchlist.Any())
            {
                throw new Exception("المنتج موجود بالفعل في قائمة المتابعة");
            }

            var watchlist = new Watchlist
            {
                UserId = userId,
                ListingId = listingId,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.Watchlists.AddAsync(watchlist);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<WatchlistDto>(watchlist);
        }

        public async Task<IEnumerable<Watchlist>> GetUserWatchlistAsync(int userId)
        {
            return await _unitOfWork.Watchlists.FindAsync(w => w.UserId == userId);
        }

        public async Task<bool> IsInWatchlistAsync(int userId, int listingId)
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