using Mzad_Palestine_Core.DTO_s.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IAuctionService
    {
        Task<AuctionDto> CreateAsync(CreateAuctionDto dto);
        Task<AuctionDto?> GetByIdAsync(int id);
        Task<IEnumerable<AuctionDto>> GetActiveAsync( );
    }
}
