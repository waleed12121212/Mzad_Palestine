using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
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
        public BidService(IBidRepository repository) => _repository = repository;

        public async Task<BidDto> CreateAsync(CreateBidDto dto)
        {
            var entity = new Bid { AuctionId = dto.AuctionId , BidAmount = dto.BidAmount , BidTime = DateTime.UtcNow };
            var created = await _repository.AddAsync(entity);
            return new BidDto(created.Id , created.AuctionId , created.UserId , created.BidAmount , created.BidTime , created.IsAutoBid , created.IsWinner , created.Status);
        }
    }

}
