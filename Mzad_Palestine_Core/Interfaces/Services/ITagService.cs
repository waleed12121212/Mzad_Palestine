using Mzad_Palestine_Core.DTO_s.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto> GetByIdAsync(int id);
        Task<TagDto> CreateAsync(CreateTagDto dto);
        Task<TagDto> UpdateAsync(int id, CreateTagDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TagDto>> SearchAsync(string query);
        Task<bool> AddTagToListingAsync(ListingTagDto dto);
        Task<bool> RemoveTagFromListingAsync(int listingId, int tagId);
    }
}
