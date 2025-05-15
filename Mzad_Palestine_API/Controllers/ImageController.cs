using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null)
                    return BadRequest(new { success = false, error = "لم يتم تحديد ملف" });

                var imagePath = await _imageService.SaveImageAsync(file);
                if (string.IsNullOrEmpty(imagePath))
                    return BadRequest(new { success = false, error = "فشل في رفع الصورة" });

                var imageUrl = _imageService.GetImageUrl(imagePath);
                return Ok(new { success = true, data = new { url = imageUrl, path = imagePath } });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete("{*path}")]
        public async Task<IActionResult> Delete(string path)
        {
            if (string.IsNullOrEmpty(path))
                return BadRequest(new { success = false, error = "المسار غير صالح" });

            var success = await _imageService.DeleteImageAsync(path);
            if (!success)
                return NotFound(new { success = false, error = "لم يتم العثور على الصورة" });

            return Ok(new { success = true, message = "تم حذف الصورة بنجاح" });
        }
    }
} 