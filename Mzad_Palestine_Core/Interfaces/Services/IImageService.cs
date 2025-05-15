using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
        Task<bool> DeleteImageAsync(string imagePath);
        string GetImageUrl(string imagePath);
    }
} 