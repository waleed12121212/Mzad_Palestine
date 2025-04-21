using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AutoBidProcessingService : IAutoBidProcessingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutoBidProcessingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessAutoBidsForAuctionAsync(int auctionId, decimal newBidAmount)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null)
                throw new InvalidOperationException("المزاد غير موجود");

            var autoBids = await _unitOfWork.AutoBids.FindAsync(ab => 
                ab.AuctionId == auctionId && 
                ab.Status == AutoBidStatus.Active && 
                ab.MaxBid > newBidAmount);

            foreach (var autoBid in autoBids.OrderByDescending(ab => ab.MaxBid))
            {
                if (auction.CurrentBid >= autoBid.MaxBid)
                    continue;

                var bidAmount = Math.Min(autoBid.MaxBid, auction.CurrentBid + auction.BidIncrement);
                var bid = new Bid
                {
                    AuctionId = auctionId,
                    UserId = autoBid.UserId,
                    BidAmount = bidAmount,
                    BidTime = DateTime.UtcNow,
                    IsAutoBid = true,
                    Status = BidStatus.Accepted
                };

                await _unitOfWork.Bids.AddAsync(bid);
                auction.CurrentBid = bidAmount;
                auction.UpdatedAt = DateTime.UtcNow;
                
                autoBid.CurrentBid = bidAmount;
                _unitOfWork.AutoBids.Update(autoBid);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
} 