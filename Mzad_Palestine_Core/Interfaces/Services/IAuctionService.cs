using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IAuctionService
    {
        Task<Auction> GetAuctionDetailsAsync(int auctionId);
        Task<IEnumerable<AuctionResponseDto>> GetUserAuctionsAsync(int userId);
        Task<IEnumerable<AuctionResponseDto>> GetOpenAuctionsAsync();
        Task<IEnumerable<AuctionResponseDto>> GetClosedAuctionsAsync();
        Task<IEnumerable<AuctionResponseDto>> SearchAuctionsAsync(AuctionSearchDto searchDto);
        Task CreateAuctionAsync(Auction auction);
        Task UpdateAuctionAsync(Auction auction, int userId);
        Task CloseAuctionAsync(int auctionId, int userId);
        Task DeleteAuctionAsync(int auctionId, int userId);
        Task<IEnumerable<AuctionResponseDto>> GetActiveAsync();
    }
}
