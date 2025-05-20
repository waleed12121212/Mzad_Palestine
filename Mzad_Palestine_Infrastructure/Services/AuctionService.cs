using Microsoft.Extensions.Logging;
using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.DTO_s.Bid;
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
using FluentValidation;
using AutoMapper;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IAutoBidRepository _autoBidRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuctionService> _logger;
        private readonly IValidator<CreateAuctionDto> _validator;
        private readonly IMapper _mapper;

        public AuctionService(
            IAuctionRepository auctionRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IBidRepository bidRepository,
            IAutoBidRepository autoBidRepository,
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork,
            ILogger<AuctionService> logger,
            IValidator<CreateAuctionDto> validator,
            IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _bidRepository = bidRepository;
            _autoBidRepository = autoBidRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Auction> CreateAsync(CreateAuctionDto dto)
        {
            var auction = new Auction
            {
                Title = dto.Title,
                Description = dto.Description,
                Address = dto.Address,
                ReservePrice = dto.ReservePrice,
                CurrentBid = dto.ReservePrice,
                BidIncrement = dto.BidIncrement,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = "Pending",
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Auctions.AddAsync(auction);
            await _unitOfWork.CompleteAsync();

            if (dto.Images != null && dto.Images.Any())
            {
                var images = dto.Images.Select((imageUrl, index) => new AuctionImage
                    {
                        AuctionId = auction.AuctionId,
                        ImageUrl = imageUrl,
                    CreatedAt = DateTime.UtcNow,
                    IsMain = index == 0 // Set first image as main image
                }).ToList();

                foreach (var image in images)
                {
                    await _auctionRepository.AddImageAsync(image);
                }
                await _unitOfWork.CompleteAsync();
            }

            return auction;
        }

        public async Task<Auction> GetByIdAsync(int id)
        {
            return await _unitOfWork.Auctions.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            var auctions = await _auctionRepository.GetQueryable()
                .ToListAsync();

            foreach (var auction in auctions)
            {
                var images = await _auctionRepository.GetAuctionImagesAsync(auction.AuctionId);
                auction.Images = images.ToList();
            }

            return auctions;
        }

        public async Task<IEnumerable<Auction>> GetByUserIdAsync(int userId)
        {
            var auctions = await _auctionRepository.GetQueryable()
                .Where(a => a.UserId == userId)
                .ToListAsync();

            foreach (var auction in auctions)
            {
                var images = await _auctionRepository.GetAuctionImagesAsync(auction.AuctionId);
                auction.Images = images.ToList();
            }

            return auctions;
        }

        public async Task<IEnumerable<Auction>> GetByCategoryAsync(int categoryId)
        {
            var auctions = await _auctionRepository.GetQueryable()
                .Where(a => a.CategoryId == categoryId)
                .ToListAsync();

            foreach (var auction in auctions)
            {
                var images = await _auctionRepository.GetAuctionImagesAsync(auction.AuctionId);
                auction.Images = images.ToList();
            }

            return auctions;
        }

        public async Task<IEnumerable<Auction>> GetActiveAsync()
        {
            var auctions = await _auctionRepository.GetQueryable()
                .Where(a => a.Status == "Active")
                .ToListAsync();

            foreach (var auction in auctions)
            {
                var images = await _auctionRepository.GetAuctionImagesAsync(auction.AuctionId);
                auction.Images = images.ToList();
            }

            return auctions;
        }

        public async Task<Auction> UpdateAsync(int id, UpdateAuctionDto dto)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(id);
            if (auction == null)
                throw new Exception("المزاد غير موجود");

            auction.Title = dto.Title;
            auction.Description = dto.Description;
            auction.Address = dto.Address;
            auction.ReservePrice = dto.ReservePrice ?? auction.ReservePrice;
            auction.BidIncrement = dto.BidIncrement ?? auction.BidIncrement;
            auction.StartDate = dto.StartDate ?? auction.StartDate;
            auction.EndDate = dto.EndDate ?? auction.EndDate;
            auction.CategoryId = dto.CategoryId ?? auction.CategoryId;
            auction.Status = dto.Status?.ToString() ?? auction.Status;
            auction.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Auctions.UpdateAsync(auction);
            await _unitOfWork.CompleteAsync();

            return auction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(id);
            if (auction == null)
                return false;

            await _unitOfWork.Auctions.DeleteAsync(auction);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<Auction> GetAuctionDetailsAsync(int auctionId)
        {
            return await _auctionRepository.GetAuctionWithBidsAsync(auctionId);
        }

        public async Task<Auction> GetAuctionWithBidsAsync(int auctionId)
        {
            return await _auctionRepository.GetAuctionWithBidsAsync(auctionId);
        }

        public async Task<IEnumerable<AuctionDto>> GetPendingAuctionsAsync()
        {
            var auctions = await _auctionRepository.GetQueryable()
                .Where(a => a.Status == "Pending")
                .ToListAsync();
            return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
        }

        public async Task<IEnumerable<AuctionDto>> GetCompletedAuctionsAsync()
        {
            var auctions = await _auctionRepository.GetQueryable()
                .Where(a => a.Status == "Completed" || a.Status == "Closed")
                .ToListAsync();
            return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
        }

        public async Task<IEnumerable<AuctionDto>> GetOpenAuctionsAsync()
        {
            var auctions = await _auctionRepository.GetOpenAuctionsAsync();
            return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
        }

        public async Task<IEnumerable<AuctionDto>> GetClosedAuctionsAsync()
        {
            var auctions = await _auctionRepository.GetClosedAuctionsAsync();
            return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
        }

        public async Task<IEnumerable<AuctionDto>> SearchAuctionsAsync(AuctionSearchDto searchDto)
        {
            var query = _auctionRepository.GetQueryable();

            if (!string.IsNullOrEmpty(searchDto.Keyword))
            {
                query = query.Where(a => a.Title.Contains(searchDto.Keyword) || a.Description.Contains(searchDto.Keyword));
            }

            if (!string.IsNullOrEmpty(searchDto.Category))
            {
                query = query.Where(a => a.Category.Name == searchDto.Category);
            }

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(a => a.CurrentBid >= searchDto.MinPrice.Value);
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(a => a.CurrentBid <= searchDto.MaxPrice.Value);
            }

            if (searchDto.StartDate.HasValue)
            {
                query = query.Where(a => a.StartDate >= searchDto.StartDate.Value);
            }

            if (searchDto.EndDate.HasValue)
            {
                query = query.Where(a => a.EndDate <= searchDto.EndDate.Value);
            }

            if (searchDto.Status.HasValue)
            {
                query = query.Where(a => a.Status == searchDto.Status.Value.ToString());
            }

            if (searchDto.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == searchDto.UserId.Value);
            }

            var auctions = await query.ToListAsync();
            return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
        }

        public async Task CreateAuctionAsync(Auction auction)
            {
            await _unitOfWork.Auctions.AddAsync(auction);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAuctionAsync(Auction auction, int userId)
        {
            if (!await _auctionRepository.IsAuctionOwnerAsync(auction.AuctionId, userId))
                throw new Exception("أنت لست مالك هذا المزاد");

            await _auctionRepository.UpdateAsync(auction);
        }

        public async Task CloseAuctionAsync(int auctionId, int userId)
        {
            var auction = await _auctionRepository.GetByIdAsync(auctionId);
                if (auction == null)
                    throw new Exception("المزاد غير موجود");

            if (!await _auctionRepository.IsAuctionOwnerAsync(auctionId, userId))
                throw new Exception("أنت لست مالك هذا المزاد");

            await _auctionRepository.CloseAuctionAsync(auctionId);
        }

        public async Task DeleteAuctionAsync(int auctionId, int userId)
        {
            if (!await _auctionRepository.IsAuctionOwnerAsync(auctionId, userId))
                throw new Exception("أنت لست مالك هذا المزاد");

            await _auctionRepository.DeleteAsync(auctionId);
        }

        public async Task<IEnumerable<AuctionDto>> GetUserAuctionsAsync(int userId)
        {
            var auctions = await _auctionRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AuctionDto>>(auctions);
        }
    }
}
