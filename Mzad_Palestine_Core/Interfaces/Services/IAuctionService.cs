using Mzad_Palestine_Core.DTO_s.Auction;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IAuctionService
    {
        Task<Auction> CreateAsync(CreateAuctionDto dto);
        Task<Auction> GetByIdAsync(int id);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<IEnumerable<Auction>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Auction>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Auction>> GetActiveAsync();
        Task<Auction> UpdateAsync(int id, UpdateAuctionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<Auction> GetAuctionDetailsAsync(int auctionId);
        Task<Auction> GetAuctionWithBidsAsync(int auctionId);
        Task<IEnumerable<AuctionDto>> GetPendingAuctionsAsync();
        Task<IEnumerable<AuctionDto>> GetCompletedAuctionsAsync();
        Task<IEnumerable<AuctionDto>> GetOpenAuctionsAsync();
        Task<IEnumerable<AuctionDto>> GetClosedAuctionsAsync();
        Task<IEnumerable<AuctionDto>> SearchAuctionsAsync(AuctionSearchDto searchDto);
        Task CreateAuctionAsync(Auction auction);
        Task UpdateAuctionAsync(Auction auction, int userId);
        Task CloseAuctionAsync(int auctionId, int userId);
        Task DeleteAuctionAsync(int auctionId, int userId);
        Task<IEnumerable<AuctionDto>> GetUserAuctionsAsync(int userId);
    }
}
