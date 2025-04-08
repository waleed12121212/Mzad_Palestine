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
    }
}
