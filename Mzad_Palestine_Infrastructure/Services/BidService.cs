using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _repository;
        private readonly ApplicationDbContext _context;
        public BidService(IBidRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<BidDto> CreateAsync(CreateBidDto dto)
        {
            var entity = new Bid { AuctionId = dto.AuctionId, BidAmount = dto.BidAmount, BidTime = DateTime.UtcNow };
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();
            var bidDto = new BidDto(entity.BidId, entity.AuctionId, entity.UserId, entity.BidAmount, entity.BidTime, entity.IsAutoBid, entity.IsWinner, entity.Status.ToString());
            return bidDto;
        }

        public Task<IEnumerable<BidDto>> GetBidsForAuctionAsync(int auctionId)
        {
            throw new NotImplementedException();
        }

        public Task<BidDto> PlaceBidAsync(CreateBidDto dto)
        {
            throw new NotImplementedException();
        }
    }

}
