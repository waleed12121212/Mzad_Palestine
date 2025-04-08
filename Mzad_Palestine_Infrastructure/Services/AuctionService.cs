using Mzad_Palestine_Core.DTO_s.Auction;
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
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _repository;
        public AuctionService(IAuctionRepository repository) => _repository = repository;

        public async Task<AuctionDto> CreateAsync(CreateAuctionDto dto)
        {
            var entity = new Auction { ListingId = dto.ListingId , StartTime = dto.StartTime , EndTime = dto.EndTime , ReservePrice = dto.ReservePrice , BidIncrement = dto.BidIncrement , ImageUrl = dto.ImageUrl , Status = "Pending" };
            var created = await _repository.AddAsync(entity);
            return new AuctionDto(created.Id , created.ListingId , created.StartTime , created.EndTime , created.ReservePrice , created.CurrentBid , created.BidIncrement , created.WinnerId , created.Status);
        }
    }
}
