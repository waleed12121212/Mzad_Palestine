using AutoMapper;
using Mzad_Palestine_Core.DTO_s.AutoBid;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class AutoBidService : IAutoBidService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AutoBidService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AutoBidDto> CreateAsync(CreateAutoBidDto dto)
        {
            var autoBid = new AutoBid
            {
                UserId = dto.UserId,
                AuctionId = dto.AuctionId,
                MaxBid = dto.MaxBid,
                Status = Mzad_Palestine_Core.Enums.AutoBidStatus.Active
            };

            await _unitOfWork.AutoBids.AddAsync(autoBid);
            await _unitOfWork.CompleteAsync();

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

        public async Task<IEnumerable<AutoBid>> GetUserAutoBidsAsync(int userId)
        {
            return await _unitOfWork.AutoBids.FindAsync(ab => ab.UserId == userId);
        }

        public async Task UpdateAutoBidAsync(AutoBid autoBid)
        {
            _unitOfWork.AutoBids.Update(autoBid);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAutoBidAsync(int id)
        {
            var autoBid = await GetAutoBidAsync(id);
            if (autoBid != null)
            {
                _unitOfWork.AutoBids.Remove(autoBid);
                await _unitOfWork.CompleteAsync();
            }
        }


    }
}