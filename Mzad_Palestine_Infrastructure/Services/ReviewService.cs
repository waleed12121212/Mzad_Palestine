using Mzad_Palestine_Core.DTO_s.Review;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto)
        {
            var review = new Review
            {
                ReviewerId = createReviewDto.ReviewerId,
                ReviewedUserId = createReviewDto.ReviewedUserId,
                ListingId = createReviewDto.ListingId,
                Comment = createReviewDto.Comment,
                Rating = createReviewDto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();

            return new ReviewDto
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewedUserId = review.ReviewedUserId,
                ListingId = review.ListingId,
                Comment = review.Comment,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewDto>> GetByListingIdAsync(int listingId)
        {
            var reviews = await _reviewRepository.FindAsync(r => r.ListingId == listingId);
            return reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewedUserId = review.ReviewedUserId,
                ListingId = review.ListingId,
                Comment = review.Comment,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            });
        }

        public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId)
        {
            var reviews = await _reviewRepository.GetReviewsForUserAsync(userId);
            return reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewedUserId = review.ReviewedUserId,
                ListingId = review.ListingId,
                Comment = review.Comment,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            });
        }

        public async Task<IEnumerable<ReviewDto>> GetByReviewerIdAsync(int reviewerId)
        {
            var reviews = await _reviewRepository.FindAsync(r => r.ReviewerId == reviewerId);
            return reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewedUserId = review.ReviewedUserId,
                ListingId = review.ListingId,
                Comment = review.Comment,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            });
        }

        public async Task<double> GetAverageRatingForUserAsync(int userId)
        {
            var reviews = await _reviewRepository.GetReviewsForUserAsync(userId);
            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }

        public async Task<ReviewDto> EditAsync(int id, UpdateReviewDto dto)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                throw new Exception("المراجعة غير موجودة");

            review.Comment = dto.Comment;
            review.Rating = dto.Rating;

            _reviewRepository.Update(review);
            await _reviewRepository.SaveChangesAsync();

            return new ReviewDto
            {
                Id = review.Id,
                ReviewerId = review.ReviewerId,
                ReviewedUserId = review.ReviewedUserId,
                ListingId = review.ListingId,
                Comment = review.Comment,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return false;

            await _reviewRepository.DeleteAsync(review);
            await _reviewRepository.SaveChangesAsync();
            return true;
        }
    }
}