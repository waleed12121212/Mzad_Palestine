using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class BidService : IBidService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BidService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BidDto> CreateBidAsync(Bid bid)
        {
            if (bid == null)
                throw new ArgumentNullException(nameof(bid), "العرض غير صالح");

            // Get the auction
            var auction = await _unitOfWork.Auctions.GetByIdAsync(bid.AuctionId);
            if (auction == null)
                throw new InvalidOperationException("المزاد غير موجود");

            // Check if auction is open
            if (auction.Status != AuctionStatus.Open)
                throw new InvalidOperationException("المزاد مغلق");

            // Check if auction has ended
            if (auction.EndTime <= DateTime.UtcNow)
                throw new InvalidOperationException("المزاد منتهي");

            // Check if bid amount is greater than current bid
            if (auction.CurrentBid >= bid.BidAmount)
                throw new InvalidOperationException("يجب أن يكون العرض أعلى من السعر الحالي");

            // Check if bid increment is valid
            var minimumBidAmount = auction.CurrentBid + auction.BidIncrement;
            if (bid.BidAmount < minimumBidAmount)
                throw new InvalidOperationException($"يجب أن يكون العرض أعلى بمقدار {auction.BidIncrement} على الأقل");

            // Check if user exists
            var user = await _unitOfWork.Users.GetByIdAsync(bid.UserId);
            if (user == null)
                throw new InvalidOperationException("المستخدم غير موجود");

            // Check if user is not the auction owner
            if (auction.UserId == bid.UserId)
                throw new InvalidOperationException("لا يمكن للمالك المزايدة على مزاده");

            try
            {
                // Set bid time
                bid.BidTime = DateTime.UtcNow;
                
                // Add the bid
                await _unitOfWork.Bids.AddAsync(bid);

                // Update auction's current bid
                auction.CurrentBid = bid.BidAmount;
                auction.UpdatedAt = DateTime.UtcNow;
                
                // Save changes
                await _unitOfWork.CompleteAsync();

                return new BidDto(
                    id: bid.BidId,
                    auctionId: bid.AuctionId,
                    userId: bid.UserId.ToString(),
                    userName: user.UserName,
                    bidAmount: bid.BidAmount,
                    createdAt: bid.BidTime,
                    isWinning: true
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("حدث خطأ أثناء إضافة العرض: " + ex.Message);
            }
        }

        public async Task<IEnumerable<BidDto>> GetAuctionBidsAsync(int auctionId)
        {
            var bids = await _unitOfWork.Bids.GetAuctionBidsAsync(auctionId);
            var bidsList = bids.ToList();
            var winningBid = await _unitOfWork.Bids.GetWinningBidAsync(auctionId);

            return bidsList.Select(bid => new BidDto(
                id: bid.BidId ,
                auctionId: bid.AuctionId ,
                userId: bid.UserId.ToString() ,
                userName: bid.User?.UserName ?? "مستخدم غير معروف" ,
                bidAmount: bid.BidAmount ,
                createdAt: bid.BidTime ,
                isWinning: winningBid != null && bid.BidId == winningBid.BidId
            ));
        }

        public async Task<IEnumerable<BidDto>> GetUserBidsAsync(int userId)
        {
            var bids = await _unitOfWork.Bids.FindAsync(b => b.UserId == userId);
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            var userName = user?.UserName ?? "مستخدم غير معروف";

            return bids.Select(bid => new BidDto(
                id: bid.BidId ,
                auctionId: bid.AuctionId ,
                userId: bid.UserId.ToString() ,
                userName: userName ,
                bidAmount: bid.BidAmount ,
                createdAt: bid.BidTime ,
                isWinning: false
            ));
        }

        public async Task DeleteBidAsync(int bidId , int userId)
        {
            var bid = await _unitOfWork.Bids.GetByIdAsync(bidId);

            if (bid == null)
                throw new InvalidOperationException("العرض غير موجود");

            if (bid.UserId != userId)
                throw new UnauthorizedAccessException("غير مصرح لك بحذف هذا العرض");

            await _unitOfWork.Bids.DeleteAsync(bid);
            await _unitOfWork.CompleteAsync();
        }
    }
}
