using Mzad_Palestine_Core.DTO_s.Bid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IBidService
    {
        Task<BidDto> PlaceBidAsync(CreateBidDto dto);
        Task<IEnumerable<BidDto>> GetBidsForAuctionAsync(int auctionId);
    }

}
