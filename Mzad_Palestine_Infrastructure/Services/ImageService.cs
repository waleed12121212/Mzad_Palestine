using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mzad_Palestine_Core.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_DIRECTORY = "uploads/images";

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return null;

                // التحقق من نوع الملف
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!IsValidImageExtension(extension))
                    throw new ArgumentException("نوع الملف غير مدعوم. الأنواع المدعومة هي: .jpg, .jpeg, .png, .gif");

                // إنشاء اسم فريد للملف
                var fileName = $"{Guid.NewGuid()}{extension}";
                var uploadPath = Path.Combine(_environment.WebRootPath, UPLOAD_DIRECTORY);
                
                // التأكد من وجود المجلد
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                // حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // إرجاع المسار النسبي للملف
                return Path.Combine(UPLOAD_DIRECTORY, fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return false;

                var fullPath = Path.Combine(_environment.WebRootPath, imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetImageUrl(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            // تحويل مسار الملف إلى URL
            return $"/{imagePath.Replace("\\", "/")}";
        }

        private bool IsValidImageExtension(string extension)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            return Array.Exists(validExtensions, e => e.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
} 