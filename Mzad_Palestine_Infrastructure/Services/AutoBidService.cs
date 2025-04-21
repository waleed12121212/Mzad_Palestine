using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mzad_Palestine_Core.DTO_s.AutoBid;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AutoBidService : IAutoBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAutoBidProcessingService _autoBidProcessingService;

        public AutoBidService(IUnitOfWork unitOfWork, IMapper mapper, IAutoBidProcessingService autoBidProcessingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _autoBidProcessingService = autoBidProcessingService;
        }

        public async Task<AutoBidDto> CreateAsync(CreateAutoBidDto dto)
        {
            // التحقق من وجود المزاد
            var auction = await _unitOfWork.Auctions.GetByIdAsync(dto.AuctionId);
            if (auction == null)
                throw new InvalidOperationException("المزاد غير موجود");

            // التحقق من حالة المزاد
            if (auction.Status != AuctionStatus.Open)
                throw new InvalidOperationException("المزاد مغلق");

            // التحقق من وقت المزاد
            if (auction.EndTime <= DateTime.UtcNow)
                throw new InvalidOperationException("المزاد منتهي");

            // التحقق من المستخدم
            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("المستخدم غير موجود");

            // التحقق من أن المستخدم ليس صاحب المزاد
            if (auction.UserId == dto.UserId)
                throw new InvalidOperationException("لا يمكن للمالك المزايدة على مزاده");

            // التحقق من عدم وجود مزايدة تلقائية سابقة للمستخدم على نفس المزاد
            var existingAutoBid = await _unitOfWork.AutoBids.GetUserAutoBidAsync(dto.UserId, dto.AuctionId);
            if (existingAutoBid != null)
                throw new InvalidOperationException("لديك بالفعل مزايدة تلقائية على هذا المزاد");

            // التحقق من أن الحد الأقصى للمزايدة أكبر من السعر الحالي
            if (dto.MaxBid <= auction.CurrentBid)
                throw new InvalidOperationException("يجب أن يكون الحد الأقصى للمزايدة أكبر من السعر الحالي");

            var autoBid = new AutoBid
            {
                UserId = dto.UserId,
                AuctionId = dto.AuctionId,
                MaxBid = dto.MaxBid,
                CurrentBid = auction.CurrentBid,
                Status = AutoBidStatus.Active
            };

            await _unitOfWork.AutoBids.AddAsync(autoBid);
            await _unitOfWork.CompleteAsync();

            // إذا كان الحد الأقصى للمزايدة أكبر من السعر الحالي، نقوم بإنشاء مزايدة جديدة
            if (dto.MaxBid > auction.CurrentBid)
            {
                await _autoBidProcessingService.ProcessAutoBidsForAuctionAsync(dto.AuctionId, auction.CurrentBid);
            }

            return _mapper.Map<AutoBidDto>(autoBid);
        }

        public async Task<AutoBidDto> GetByIdAsync(int id)
        {
            var autoBid = await _unitOfWork.AutoBids.GetByIdAsync(id);
            return autoBid == null ? null : _mapper.Map<AutoBidDto>(autoBid);
        }

        public async Task<AutoBid> GetAutoBidAsync(int id)
        {
            return await _unitOfWork.AutoBids.GetByIdAsync(id);
        }

        public async Task<IEnumerable<AutoBidDto>> GetUserAutoBidsAsync(int userId)
        {
            var autoBids = await _unitOfWork.AutoBids.FindAsync(ab => ab.UserId == userId);
            return _mapper.Map<IEnumerable<AutoBidDto>>(autoBids);
        }

        public async Task<AutoBidDto> GetUserAutoBidForAuctionAsync(int userId, int auctionId)
        {
            var autoBid = await _unitOfWork.AutoBids.GetUserAutoBidAsync(userId, auctionId);
            return autoBid == null ? null : _mapper.Map<AutoBidDto>(autoBid);
        }

        public async Task UpdateAutoBidAsync(int id, decimal maxBid)
        {
            var autoBid = await _unitOfWork.AutoBids.GetByIdAsync(id);
            if (autoBid == null)
                throw new InvalidOperationException("المزايدة التلقائية غير موجودة");

            var auction = await _unitOfWork.Auctions.GetByIdAsync(autoBid.AuctionId);
            if (auction == null)
                throw new InvalidOperationException("المزاد غير موجود");

            if (maxBid <= auction.CurrentBid)
                throw new InvalidOperationException("يجب أن يكون الحد الأقصى للمزايدة أكبر من السعر الحالي");

            autoBid.MaxBid = maxBid;
            _unitOfWork.AutoBids.Update(autoBid);
            await _unitOfWork.CompleteAsync();

            // إذا كان الحد الأقصى الجديد أكبر من السعر الحالي، نقوم بإنشاء مزايدة جديدة
            if (maxBid > auction.CurrentBid)
            {
                await _autoBidProcessingService.ProcessAutoBidsForAuctionAsync(autoBid.AuctionId, auction.CurrentBid);
            }
        }

        public async Task DeleteAutoBidAsync(int id, int userId)
        {
            var autoBid = await _unitOfWork.AutoBids.GetByIdAsync(id);
            if (autoBid == null)
                throw new InvalidOperationException("المزايدة التلقائية غير موجودة");

            if (autoBid.UserId != userId)
                throw new InvalidOperationException("لا يمكنك حذف مزايدة تلقائية لمستخدم آخر");

            await _unitOfWork.AutoBids.DeleteAsync(autoBid);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ProcessAutoBidsForAuctionAsync(int auctionId, decimal newBidAmount)
        {
            await _autoBidProcessingService.ProcessAutoBidsForAuctionAsync(auctionId, newBidAmount);
        }
    }
}