using Mzad_Palestine_Core.DTO_s.Bid;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IBidService
    {
        Task<BidDto> CreateBidAsync(Bid bid);
        Task<IEnumerable<BidDto>> GetAuctionBidsAsync(int auctionId);
        Task<IEnumerable<BidDto>> GetUserBidsAsync(int userId);
        Task DeleteBidAsync(int bidId, int userId);
    }
}
