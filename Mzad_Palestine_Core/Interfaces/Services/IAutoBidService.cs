using Mzad_Palestine_Core.DTO_s.AutoBid;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IAutoBidService
    {
        Task<AutoBidDto> CreateAsync(CreateAutoBidDto dto);
        Task<AutoBidDto> GetByIdAsync(int id);
        Task<IEnumerable<AutoBidDto>> GetUserAutoBidsAsync(int userId);
        Task<AutoBidDto> GetUserAutoBidForAuctionAsync(int userId, int auctionId);
        Task UpdateAutoBidAsync(int id, decimal maxBid);
        Task DeleteAutoBidAsync(int id, int userId);
        Task ProcessAutoBidsForAuctionAsync(int auctionId, decimal newBidAmount);
    }

}
