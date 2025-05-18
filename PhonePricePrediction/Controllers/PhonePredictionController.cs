using Microsoft.AspNetCore.Mvc;
using PhonePricePrediction.Models;

namespace PhonePricePrediction.Controllers;

[ApiController]
[Route("[controller]")]
public class PhonePredictionController : ControllerBase
{
    private readonly ILogger<PhonePredictionController> _logger;

    public PhonePredictionController(ILogger<PhonePredictionController> logger)
    {
        _logger = logger;
    }

    [HttpPost("predict")]
    public IActionResult Predict([FromBody] PhoneSpecs specs)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // حساب السعر بناءً على المواصفات
            double base_price = 1000;
            double price = (
                specs.BatteryCapacity * 0.1 +    // كل 1000 مللي أمبير تضيف 100 شيكل
                specs.DisplaySize * 100 +        // كل بوصة تضيف 100 شيكل
                specs.Storage * 0.5 +            // كل 1 جيجا تخزين تضيف 0.5 شيكل
                specs.Ram * 50 +                 // كل 1 جيجا رام تضيف 50 شيكل
                specs.RefreshRate * 2 +          // كل 1 هرتز معدل تحديث تضيف 2 شيكل
                int.Parse(specs.FrontCamera.Replace("MP", "")) * 10 +  // كل 1 ميجابكسل كاميرا أمامية تضيف 10 شيكل
                int.Parse(specs.RearCamera.Split('+')[0].Replace("MP", "").Trim()) * 5 +  // كل 1 ميجابكسل كاميرا خلفية تضيف 5 شيكل
                specs.ChargingSpeed * 3 +        // كل 1 واط سرعة شحن تضيف 3 شيكل
                base_price                       // السعر الأساسي
            );

            // إضافة علاوة إذا كان الهاتف يدعم توسيع الرام
            if (specs.RamExpandable)
            {
                price *= 1.1; // زيادة 10% للسعر
            }

            return Ok(new { 
                price = Math.Round(price, 2),
                currency = "ILS",
                deviceName = specs.DeviceName,
                timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "حدث خطأ أثناء التنبؤ بسعر الهاتف");
            return StatusCode(500, new { error = "حدث خطأ أثناء التنبؤ بالسعر" });
        }
    }
} 