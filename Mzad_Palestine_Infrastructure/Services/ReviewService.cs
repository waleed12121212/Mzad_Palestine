using Mzad_Palestine_Core.DTO_s.Review;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            var review = new Review
            {
                ReviewerId = createReviewDto.ReviewerId ,
                ReviewedUserId = createReviewDto.ReviewedUserId ,
                Comment = createReviewDto.Comment ,
                Rating = createReviewDto.Rating ,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);

            return new ReviewDto
            {
                Id = review.Id ,
                ReviewerId = review.ReviewerId ,
                ReviewedUserId = review.ReviewedUserId ,
                Comment = review.Comment ,
                Rating = review.Rating ,
                CreatedAt = review.CreatedAt
            };
        }

        public Task<IEnumerable<ReviewDto>> GetByListingIdAsync(int listingId)
        {
            throw new NotImplementedException();
        }
    }
}