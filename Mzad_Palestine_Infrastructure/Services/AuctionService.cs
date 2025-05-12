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
using Mzad_Palestine_Core.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _repository;
        private readonly IBidRepository _bidRepository;
        private readonly IAutoBidRepository _autoBidRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuctionService(
            IAuctionRepository repository,
            IBidRepository bidRepository,
            IAutoBidRepository autoBidRepository,
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _bidRepository = bidRepository;
            _autoBidRepository = autoBidRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuctionResponseDto> CreateAsync(CreateAuctionDto dto)
        {
            var entity = new Auction
            {
                ListingId = dto.ListingId,
                Name = dto.Name,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ReservePrice = dto.ReservePrice,
                BidIncrement = dto.BidIncrement,
                ImageUrl = dto.ImageUrl,
                Status = AuctionStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var createdAuction = await _repository.GetByIdAsync(entity.AuctionId);
            return new AuctionResponseDto
            {
                AuctionId = createdAuction.AuctionId,
                Name = createdAuction.Name,
                CategoryName = createdAuction.Listing?.Category?.Name,
                CategoryId = createdAuction.Listing?.CategoryId ?? 0,
                ReservePrice = createdAuction.ReservePrice,
                CurrentBid = createdAuction.CurrentBid,
                BidIncrement = createdAuction.BidIncrement,
                StartTime = createdAuction.StartTime,
                EndTime = createdAuction.EndTime,
                ImageUrl = createdAuction.ImageUrl,
                Status = createdAuction.Status,
                BidsCount = createdAuction.Bids?.Count ?? 0,
                WinnerName = createdAuction.Winner?.UserName
            };
        }

        public async Task<Auction> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<AuctionResponseDto>> GetActiveAsync()
        {
            try
            {
                var auctions = await _repository.GetActiveAsync();
                return auctions.Select(auction => new AuctionResponseDto
                {
                    AuctionId = auction.AuctionId,
                    ListingId = auction.ListingId,
                    Name = auction.Name,
                    CategoryName = auction.Listing?.Category?.Name,
                    CategoryId = auction.Listing?.CategoryId ?? 0,
                    ReservePrice = auction.ReservePrice,
                    CurrentBid = auction.CurrentBid,
                    BidIncrement = auction.BidIncrement,
                    StartTime = auction.StartTime,
                    EndTime = auction.EndTime,
                    ImageUrl = auction.ImageUrl,
                    Status = auction.Status,
                    BidsCount = auction.Bids?.Count ?? 0,
                    WinnerName = auction.Winner?.UserName
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب المزادات النشطة: {ex.Message}");
            }
        }

        public async Task<AuctionResponseDto> UpdateAsync(int id, UpdateAuctionDto dto)
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
            await _unitOfWork.CompleteAsync();

            var updatedAuction = await _repository.GetByIdAsync(id);
            return new AuctionResponseDto
            {
                AuctionId = updatedAuction.AuctionId,
                Name = updatedAuction.Name,
                CategoryName = updatedAuction.Listing?.Category?.Name,
                CategoryId = updatedAuction.Listing?.CategoryId ?? 0,
                ReservePrice = updatedAuction.ReservePrice,
                CurrentBid = updatedAuction.CurrentBid,
                BidIncrement = updatedAuction.BidIncrement,
                StartTime = updatedAuction.StartTime,
                EndTime = updatedAuction.EndTime,
                ImageUrl = updatedAuction.ImageUrl,
                Status = updatedAuction.Status,
                BidsCount = updatedAuction.Bids?.Count ?? 0,
                WinnerName = updatedAuction.Winner?.UserName
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
            var auction = await _repository.GetAuctionWithBidsAsync(auctionId);
            if (auction == null) return null;

            // Create a new auction object without circular references
            var auctionDto = new Auction
            {
                AuctionId = auction.AuctionId,
                ListingId = auction.ListingId,
                UserId = auction.UserId,
                Name = auction.Name,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                ReservePrice = auction.ReservePrice,
                CurrentBid = auction.CurrentBid,
                BidIncrement = auction.BidIncrement,
                WinnerId = auction.WinnerId,
                Status = auction.Status,
                ImageUrl = auction.ImageUrl,
                CreatedAt = auction.CreatedAt,
                UpdatedAt = auction.UpdatedAt,
                Listing = auction.Listing != null ? new Listing
                {
                    ListingId = auction.Listing.ListingId,
                    CategoryId = auction.Listing.CategoryId,
                    Category = auction.Listing.Category != null ? new Category
                    {
                        Id = auction.Listing.Category.Id,
                        Name = auction.Listing.Category.Name
                    } : null
                } : null,
                Bids = auction.Bids?.Select(b => new Bid
                {
                    BidId = b.BidId,
                    AuctionId = b.AuctionId,
                    UserId = b.UserId,
                    BidAmount = b.BidAmount,
                    BidTime = b.BidTime,
                    IsAutoBid = b.IsAutoBid,
                    IsWinner = b.IsWinner,
                    Status = b.Status,
                    User = b.User != null ? new User
                    {
                        Id = b.User.Id,
                        UserName = b.User.UserName,
                        Email = b.User.Email
                    } : null
                }).ToList(),
                Winner = auction.Winner != null ? new User
                {
                    Id = auction.Winner.Id,
                    UserName = auction.Winner.UserName,
                    Email = auction.Winner.Email
                } : null
            };

            return auctionDto;
        }

        public async Task<IEnumerable<AuctionResponseDto>> GetUserAuctionsAsync(int userId)
        {
            try
            {
                return await _repository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب مزادات المستخدم: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AuctionResponseDto>> GetOpenAuctionsAsync()
        {
            try
            {
                var auctions = await _repository.GetOpenAuctionsAsync();
                return auctions.Select(auction => new AuctionResponseDto
                {
                    AuctionId = auction.AuctionId,
                    ListingId = auction.ListingId,
                    Name = auction.Name,
                    CategoryName = auction.Listing?.Category?.Name,
                    CategoryId = auction.Listing?.CategoryId ?? 0,
                    ReservePrice = auction.ReservePrice,
                    CurrentBid = auction.CurrentBid,
                    BidIncrement = auction.BidIncrement,
                    StartTime = auction.StartTime,
                    EndTime = auction.EndTime,
                    ImageUrl = auction.ImageUrl,
                    Status = auction.Status,
                    BidsCount = auction.Bids?.Count ?? 0,
                    WinnerName = auction.Winner?.UserName
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب المزادات المفتوحة: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AuctionResponseDto>> GetClosedAuctionsAsync()
        {
            try
            {
                var auctions = await _repository.GetClosedAuctionsAsync();
                return auctions.Select(auction => new AuctionResponseDto
                {
                    AuctionId = auction.AuctionId,
                    ListingId = auction.ListingId,
                    Name = auction.Name,
                    CategoryName = auction.Listing?.Category?.Name,
                    CategoryId = auction.Listing?.CategoryId ?? 0,
                    ReservePrice = auction.ReservePrice,
                    CurrentBid = auction.CurrentBid,
                    BidIncrement = auction.BidIncrement,
                    StartTime = auction.StartTime,
                    EndTime = auction.EndTime,
                    ImageUrl = auction.ImageUrl,
                    Status = auction.Status,
                    BidsCount = auction.Bids?.Count ?? 0,
                    WinnerName = auction.Winner?.UserName
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء جلب المزادات المغلقة: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AuctionResponseDto>> SearchAuctionsAsync(AuctionSearchDto searchDto)
        {
            try
            {
                return await _repository.SearchAsync(searchDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء البحث عن المزادات: {ex.Message}");
            }
        }

        public async Task CreateAuctionAsync(Auction auction)
        {
            try
            {
                auction.CreatedAt = DateTime.UtcNow;
                auction.Status = AuctionStatus.Open;
                await _repository.AddAsync(auction);
                var result = await _unitOfWork.CompleteAsync();
                
                if (result <= 0)
                {
                    throw new Exception("فشل في حفظ المزاد");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء إنشاء المزاد: {ex.Message}");
            }
        }

        public async Task UpdateAuctionAsync(Auction auction, int userId)
        {
            try
            {
                var existingAuction = await _repository.GetByIdAsync(auction.AuctionId);
                if (existingAuction == null)
                    throw new Exception("المزاد غير موجود");

                if (existingAuction.UserId != userId)
                    throw new Exception("غير مصرح لك بتحديث هذا المزاد");

                existingAuction.StartTime = auction.StartTime;
                existingAuction.EndTime = auction.EndTime;
                existingAuction.ReservePrice = auction.ReservePrice;
                existingAuction.BidIncrement = auction.BidIncrement;
                existingAuction.ImageUrl = auction.ImageUrl;
                existingAuction.UpdatedAt = DateTime.UtcNow;

                _repository.Update(existingAuction);
                var result = await _unitOfWork.CompleteAsync();
                
                if (result <= 0)
                {
                    throw new Exception("فشل في تحديث المزاد");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء تحديث المزاد: {ex.Message}");
            }
        }

        public async Task CloseAuctionAsync(int auctionId, int userId)
        {
            try
            {
                var auction = await _repository.GetByIdAsync(auctionId);
                if (auction == null)
                    throw new Exception("المزاد غير موجود");

                if (auction.UserId != userId)
                    throw new Exception("غير مصرح لك بإغلاق هذا المزاد");

                auction.Status = AuctionStatus.Closed;
                auction.UpdatedAt = DateTime.UtcNow;
                _repository.Update(auction);

                // تحديد الفائز
                var winningBid = await _bidRepository.GetHighestBidAsync(auctionId);
                if (winningBid != null)
                {
                    winningBid.IsWinner = true;
                    _bidRepository.Update(winningBid);

                    auction.WinnerId = winningBid.UserId;
                    auction.CurrentBid = winningBid.BidAmount;

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

                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    throw new Exception("فشل في إغلاق المزاد");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء إغلاق المزاد: {ex.Message}");
            }
        }

        public async Task DeleteAuctionAsync(int auctionId, int userId)
        {
            try
            {
                var auction = await _repository.GetByIdAsync(auctionId);
                if (auction == null)
                    throw new Exception("المزاد غير موجود");

                if (auction.UserId != userId)
                    throw new Exception("غير مصرح لك بحذف هذا المزاد");

                await _repository.DeleteAsync(auction);
                var result = await _unitOfWork.CompleteAsync();
                
                if (result <= 0)
                {
                    throw new Exception("فشل في حذف المزاد");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء حذف المزاد: {ex.Message}");
            }
        }

        public async Task<Auction> GetAuctionWithBidsAsync(int auctionId)
        {
            return await _repository.GetAuctionWithBidsAsync(auctionId);
        }
    }
}
