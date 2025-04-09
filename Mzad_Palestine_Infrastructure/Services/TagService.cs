using Mzad_Palestine_Core.DTO_s.Tag;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Interfaces.Common;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Tag> CreateTagAsync(CreateTagDto dto)
        {
            var tag = new Tag
            {
                Name = dto.Name ,
                Description = dto.Description
            };

            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.CompleteAsync();

            return tag;
        }

        public async Task<Tag> GetTagAsync(int id)
        {
            return await _unitOfWork.Tags.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync( )
        {
            return await _unitOfWork.Tags.GetAllAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _unitOfWork.Tags.Update(tag);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await GetTagAsync(id);
            if (tag != null)
            {
                _unitOfWork.Tags.Remove(tag);
                await _unitOfWork.CompleteAsync();
            }
        }

        public Task<IEnumerable<TagDto>> GetAllAsync( )
        {
            throw new NotImplementedException();
        }
    }
}