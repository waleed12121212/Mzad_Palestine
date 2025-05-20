using Mzad_Palestine_Core.DTO_s.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateAsync(CreateReviewDto dto);
        Task<IEnumerable<ReviewDto>> GetByListingIdAsync(int listingId);
        Task<IEnumerable<ReviewDto>> GetByAuctionIdAsync(int auctionId);
        Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<ReviewDto>> GetByReviewerIdAsync(int reviewerId);
        Task<double> GetAverageRatingForUserAsync(int userId);
        Task<ReviewDto> EditAsync(int id, UpdateReviewDto dto);
        Task<bool> DeleteAsync(int id);
        Task<(double averageRating, int totalReviews)> GetListingAverageRatingAsync(int listingId);
        Task<(double averageRating, int totalReviews)> GetAuctionAverageRatingAsync(int auctionId);
        Task<ReviewDto> GetByIdAsync(int id);
    }
}
