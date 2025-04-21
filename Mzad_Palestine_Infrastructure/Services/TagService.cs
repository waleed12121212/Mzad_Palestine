using Mzad_Palestine_Core.DTO_s.Tag;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _unitOfWork.Tags.GetAllAsync();
            return tags.Select(t => new TagDto
            {
                Id = t.TagId,
                Name = t.Name
            });
        }

        public async Task<TagDto> GetByIdAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
                return null;

            return new TagDto
            {
                Id = tag.TagId,
                Name = tag.Name
            };
        }

        public async Task<TagDto> CreateAsync(CreateTagDto dto)
        {
            // Check if tag with same name exists
            var existingTag = await _unitOfWork.Tags.FindAsync(t => t.Name.ToLower() == dto.Name.ToLower());
            if (existingTag.Any())
            {
                throw new Exception("يوجد وسم بنفس الاسم");
            }

            var tag = new Tag
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.CompleteAsync();

            return new TagDto
            {
                Id = tag.TagId,
                Name = tag.Name
            };
        }

        public async Task<TagDto> UpdateAsync(int id, CreateTagDto dto)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
                return null;

            // Check if another tag with same name exists
            var existingTag = await _unitOfWork.Tags.FindAsync(t => t.Name.ToLower() == dto.Name.ToLower() && t.TagId != id);
            if (existingTag.Any())
            {
                throw new Exception("يوجد وسم آخر بنفس الاسم");
            }

            tag.Name = dto.Name;
            tag.Description = dto.Description;

            _unitOfWork.Tags.Update(tag);
            await _unitOfWork.CompleteAsync();

            return new TagDto
            {
                Id = tag.TagId,
                Name = tag.Name
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
                return false;

            await _unitOfWork.Tags.DeleteAsync(tag);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<TagDto>> SearchAsync(string query)
        {
            var tags = await _unitOfWork.Tags.SearchTagsAsync(query);
            return tags.Select(t => new TagDto
            {
                Id = t.TagId,
                Name = t.Name
            });
        }

        public async Task<bool> AddTagToListingAsync(ListingTagDto dto)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(dto.ListingId);
            var tag = await _unitOfWork.Tags.GetByIdAsync(dto.TagId);

            if (listing == null || tag == null)
                return false;

            var listingTag = new ListingTag
            {
                ListingId = dto.ListingId,
                TagId = dto.TagId
            };

            // Check if the relationship already exists
            var existingRelation = await _unitOfWork.Tags.FindAsync(t => 
                t.ListingTags.Any(lt => lt.ListingId == dto.ListingId && lt.TagId == dto.TagId));

            if (existingRelation.Any())
                return false;

            listing.ListingTags.Add(listingTag);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> RemoveTagFromListingAsync(int listingId, int tagId)
        {
            var listing = await _unitOfWork.Listings.GetByIdAsync(listingId);
            if (listing == null)
                return false;

            var listingTag = listing.ListingTags.FirstOrDefault(lt => lt.TagId == tagId);
            if (listingTag == null)
                return false;

            listing.ListingTags.Remove(listingTag);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}