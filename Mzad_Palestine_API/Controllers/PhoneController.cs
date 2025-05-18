using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Mzad_Palestine_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly IPhonePredictionService _phonePredictionService;

        public PhoneController(IPhonePredictionService phonePredictionService)
        {
            _phonePredictionService = phonePredictionService;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictPrice([FromBody] Phone phone)
        {
            try
            {
                // التحقق من صحة البيانات
                if (!ValidatePhoneData(phone, out string errorMessage))
                {
                    return BadRequest(new { error = errorMessage });
                }

                var predictedPrice = await _phonePredictionService.PredictPrice(phone);
                return Ok(new { price = predictedPrice });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePhoneSpecs([FromBody] Phone phone)
        {
            try
            {
                // التحقق من صحة البيانات
                if (!ValidatePhoneData(phone, out string errorMessage))
                {
                    return BadRequest(new { error = errorMessage });
                }

                var savedPhone = await _phonePredictionService.SavePhoneSpecs(phone);
                return Ok(savedPhone);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private bool ValidatePhoneData(Phone phone, out string errorMessage)
        {
            errorMessage = string.Empty;

            // التحقق من اسم الجهاز
            if (string.IsNullOrWhiteSpace(phone.DeviceName) || phone.DeviceName == "string")
            {
                errorMessage = "يرجى إدخال اسم الجهاز بشكل صحيح";
                return false;
            }

            // التحقق من سعة البطارية
            if (phone.BatteryCapacity <= 0 || phone.BatteryCapacity > 10000)
            {
                errorMessage = "سعة البطارية يجب أن تكون بين 1 و 10000 مللي أمبير";
                return false;
            }

            // التحقق من حجم الشاشة
            if (phone.DisplaySize <= 0 || phone.DisplaySize > 10)
            {
                errorMessage = "حجم الشاشة يجب أن يكون بين 1 و 10 بوصة";
                return false;
            }

            // التحقق من التخزين
            if (phone.Storage <= 0 || phone.Storage > 1024)
            {
                errorMessage = "سعة التخزين يجب أن تكون بين 1 و 1024 جيجابايت";
                return false;
            }

            // التحقق من الرام
            if (phone.Ram <= 0 || phone.Ram > 32)
            {
                errorMessage = "سعة الرام يجب أن تكون بين 1 و 32 جيجابايت";
                return false;
            }

            // التحقق من معدل التحديث
            if (phone.RefreshRate <= 0 || phone.RefreshRate > 240)
            {
                errorMessage = "معدل التحديث يجب أن يكون بين 1 و 240 هرتز";
                return false;
            }

            // التحقق من الكاميرا الأمامية
            if (!IsValidCameraSpec(phone.FrontCamera))
            {
                errorMessage = "مواصفات الكاميرا الأمامية غير صحيحة";
                return false;
            }

            // التحقق من الكاميرا الخلفية
            if (!IsValidCameraSpec(phone.RearCamera))
            {
                errorMessage = "مواصفات الكاميرا الخلفية غير صحيحة";
                return false;
            }

            // التحقق من سرعة الشحن
            if (phone.ChargingSpeed <= 0 || phone.ChargingSpeed > 200)
            {
                errorMessage = "سرعة الشحن يجب أن تكون بين 1 و 200 واط";
                return false;
            }

            return true;
        }

        private bool IsValidCameraSpec(string cameraSpec)
        {
            if (string.IsNullOrWhiteSpace(cameraSpec) || cameraSpec == "string")
                return false;

            // التحقق من تنسيق الكاميرا (مثال: "12MP" أو "12MP+12MP")
            var pattern = @"^(\d+MP)(\+\d+MP)*$";
            return Regex.IsMatch(cameraSpec.Trim(), pattern);
        }
    }
} 