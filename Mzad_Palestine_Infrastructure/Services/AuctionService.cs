using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _repository;
        private readonly IBidRepository _bidRepository;
        private readonly IAutoBidRepository _autoBidRepository;
        private readonly INotificationRepository _notificationRepository;

        public AuctionService(
            IAuctionRepository repository,
            IBidRepository bidRepository,
            IAutoBidRepository autoBidRepository,
            INotificationRepository notificationRepository)
        {
            _repository = repository;
            _bidRepository = bidRepository;
            _autoBidRepository = autoBidRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<AuctionDto> CreateAsync(CreateAuctionDto dto)
        {
            var entity = new Auction
            {
                ListingId = dto.ListingId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ReservePrice = dto.ReservePrice,
                BidIncrement = dto.BidIncrement,
                ImageUrl = dto.ImageUrl,
                Status = AuctionStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);

            return new AuctionDto
            {
                Id = entity.AuctionId,
                ListingId = entity.ListingId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ReservePrice = entity.ReservePrice,
                CurrentBid = entity.CurrentBid,
                BidIncrement = entity.BidIncrement,
                WinnerId = entity.WinnerId,
                Status = entity.Status,
                ImageUrl = entity.ImageUrl
            };
        }

        public async Task<AuctionDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new AuctionDto
            {
                Id = entity.AuctionId,
                ListingId = entity.ListingId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ReservePrice = entity.ReservePrice,
                CurrentBid = entity.CurrentBid,
                BidIncrement = entity.BidIncrement,
                WinnerId = entity.WinnerId,
                Status = entity.Status,
                ImageUrl = entity.ImageUrl
            };
        }

        public async Task<IEnumerable<AuctionDto>> GetActiveAsync()
        {
            var entities = await _repository.GetActiveAsync();
            return entities.Select(entity => new AuctionDto
            {
                Id = entity.AuctionId,
                ListingId = entity.ListingId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ReservePrice = entity.ReservePrice,
                CurrentBid = entity.CurrentBid,
                BidIncrement = entity.BidIncrement,
                WinnerId = entity.WinnerId,
                Status = entity.Status,
                ImageUrl = entity.ImageUrl
            });
        }

        public async Task<AuctionDto?> UpdateAsync(int id, UpdateAuctionDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            if (dto.StartTime.HasValue)
                entity.StartTime = dto.StartTime.Value;
            if (dto.EndTime.HasValue)
                entity.EndTime = dto.EndTime.Value;
            if (dto.ReservePrice.HasValue)
                entity.ReservePrice = dto.ReservePrice.Value;
            if (dto.BidIncrement.HasValue)
                entity.BidIncrement = dto.BidIncrement.Value;
            if (!string.IsNullOrEmpty(dto.ImageUrl))
                entity.ImageUrl = dto.ImageUrl;

            await _repository.UpdateAsync(entity);

            return new AuctionDto
            {
                Id = entity.AuctionId,
                ListingId = entity.ListingId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ReservePrice = entity.ReservePrice,
                CurrentBid = entity.CurrentBid,
                BidIncrement = entity.BidIncrement,
                WinnerId = entity.WinnerId,
                Status = entity.Status,
                ImageUrl = entity.ImageUrl
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            await _repository.DeleteAsync(entity);
            return true;
        }

        public async Task<IEnumerable<AuctionDto>> SearchAsync(AuctionSearchDto searchDto)
        {
            var auctions = await _repository.SearchAsync(searchDto);
            return auctions.Select(entity => new AuctionDto
            {
                Id = entity.AuctionId,
                ListingId = entity.ListingId,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ReservePrice = entity.ReservePrice,
                CurrentBid = entity.CurrentBid,
                BidIncrement = entity.BidIncrement,
                WinnerId = entity.WinnerId,
                Status = entity.Status,
                ImageUrl = entity.ImageUrl
            });
        }

        public async Task<Auction> GetAuctionDetailsAsync(int auctionId)
        {
            return await _repository.GetByIdAsync(auctionId);
        }

        public async Task<IEnumerable<Auction>> GetUserAuctionsAsync(int userId)
        {
            var auctions = await _repository.GetAllAsync();
            return auctions.Where(a => a.Listing.UserId == userId);
        }

        public async Task<IEnumerable<Auction>> GetOpenAuctionsAsync()
        {
            return await _repository.GetOpenAuctionsAsync();
        }

        public async Task<IEnumerable<Auction>> GetClosedAuctionsAsync()
        {
            return await _repository.GetClosedAuctionsAsync();
        }

        public async Task<IEnumerable<Auction>> SearchAuctionsAsync(AuctionSearchDto searchDto)
        {
            return await _repository.SearchAsync(searchDto);
        }

        public async Task CreateAuctionAsync(Auction auction)
        {
            auction.CreatedAt = DateTime.UtcNow;
            auction.Status = AuctionStatus.Open;
            await _repository.AddAsync(auction);
        }

        public async Task UpdateAuctionAsync(Auction auction, int userId)
        {
            var existingAuction = await _repository.GetByIdAsync(auction.AuctionId);
            if (existingAuction == null) return;

            if (existingAuction.Listing.UserId != userId) return;

            existingAuction.StartTime = auction.StartTime;
            existingAuction.EndTime = auction.EndTime;
            existingAuction.ReservePrice = auction.ReservePrice;
            existingAuction.BidIncrement = auction.BidIncrement;
            existingAuction.ImageUrl = auction.ImageUrl;

            await _repository.UpdateAsync(existingAuction);
        }

        public async Task CloseAuctionAsync(int auctionId, int userId)
        {
            var auction = await _repository.GetByIdAsync(auctionId);
            if (auction == null) return;

            if (auction.Listing.UserId != userId) return;

            auction.Status = AuctionStatus.Closed;
            _repository.Update(auction);

            // تحديد الفائز
            var bids = await _bidRepository.GetBidsByAuctionAsync(auctionId);
            var winningBid = await _bidRepository.GetHighestBidAsync(auctionId);
            if (winningBid != null)
            {
                winningBid.IsWinner = true;
                _bidRepository.Update(winningBid);

                // إرسال إشعار للفائز
                var notification = new Notification
                {
                    UserId = winningBid.UserId,
                    RelatedId = auctionId,
                    Message = "مبروك! لقد فزت بالمزاد",
                    Type = NotificationType.General,
                    Status = NotificationStatus.Unread,
                    CreatedAt = DateTime.UtcNow
                };

                await _notificationRepository.AddAsync(notification);
            }
        }

        public async Task DeleteAuctionAsync(int auctionId, int userId)
        {
            var auction = await _repository.GetByIdAsync(auctionId);
            if (auction == null) return;

            if (auction.Listing.UserId != userId) return;

            await _repository.DeleteAsync(auction);
        }
    }
}
