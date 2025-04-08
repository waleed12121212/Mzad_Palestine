using Mzad_Palestine_Core.DTO_s.Review;
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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        public ReviewService(IReviewRepository repository) => _repository = repository;

        public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            var entity = new Review { RevieweeId = dto.RevieweeId , ListingId = dto.ListingId , Rating = dto.Rating , Comment = dto.Comment };
            var created = await _repository.AddAsync(entity);
            return new ReviewDto(created.Id , created.ReviewerId , created.RevieweeId , created.ListingId , created.Rating , created.Comment);
        }
    }
}
