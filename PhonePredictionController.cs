using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Python.Runtime;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.IO;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PhonePredictionController : ControllerBase
{
    private readonly ILogger<PhonePredictionController> _logger;
    private readonly string _modelPath;
    private readonly string _scalerPath;
    private readonly string _pythonScriptPath;

    public PhonePredictionController(ILogger<PhonePredictionController> logger)
    {
        _logger = logger;
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _modelPath = Path.Combine(baseDir, "phone_price_model.pkl");
        _scalerPath = Path.Combine(baseDir, "phone_price_scaler.pkl");
        _pythonScriptPath = Path.Combine(baseDir, "python", "phone_prediction_model.py");

        _logger.LogInformation($"Base Directory: {baseDir}");
        _logger.LogInformation($"Model Path: {_modelPath}");
        _logger.LogInformation($"Scaler Path: {_scalerPath}");
        _logger.LogInformation($"Python Script Path: {_pythonScriptPath}");

        // التأكد من وجود ملفات النموذج
        if (!File.Exists(_modelPath) || !File.Exists(_scalerPath))
        {
            _logger.LogWarning("ملفات النموذج غير موجودة. جاري إنشاء النموذج...");
            try
            {
                if (!File.Exists(_pythonScriptPath))
                {
                    throw new FileNotFoundException($"ملف النموذج Python غير موجود في: {_pythonScriptPath}");
                }

                using (Py.GIL())
                {
                    dynamic sys = Py.Import("sys");
                    sys.path.append(Path.GetDirectoryName(_pythonScriptPath));
                    
                    dynamic phone_prediction = Py.Import("phone_prediction_model");
                    phone_prediction.train_model();
                    
                    _logger.LogInformation("تم إنشاء النموذج بنجاح");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشل في إنشاء النموذج");
                throw;
            }
        }
    }

    /// <summary>
    /// التنبؤ بسعر الهاتف بناءً على مواصفاته
    /// </summary>
    /// <param name="specs">مواصفات الهاتف</param>
    /// <returns>السعر المتوقع للهاتف</returns>
    /// <response code="200">تم التنبؤ بالسعر بنجاح</response>
    /// <response code="500">حدث خطأ أثناء التنبؤ بالسعر</response>
    [HttpPost("predict")]
    [ProducesResponseType(typeof(PredictionResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> Predict([FromBody] PhoneSpecs specs)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"تم استلام طلب تنبؤ للجهاز: {specs.DeviceName}");

            // تحويل المواصفات إلى قاموس
            var phoneSpecs = new Dictionary<string, object>
            {
                { "battery_capacity", specs.BatteryCapacity },
                { "display_size", specs.DisplaySize },
                { "storage", specs.Storage },
                { "ram", specs.Ram },
                { "refresh_rate", specs.RefreshRate },
                { "front_camera_mp", int.Parse(specs.FrontCamera.Replace("MP", "")) },
                { "rear_camera_mp", int.Parse(specs.RearCamera.Split('+')[0].Replace("MP", "").Trim()) },
                { "charging_speed", specs.ChargingSpeed }
            };

            _logger.LogInformation($"المواصفات المحولة: {JsonSerializer.Serialize(phoneSpecs)}");

            // تشغيل نموذج Python
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(Path.GetDirectoryName(_pythonScriptPath));
                
                dynamic phone_prediction = Py.Import("phone_prediction_model");
                dynamic result = phone_prediction.predict_price(phoneSpecs);

                _logger.LogInformation($"تم التنبؤ بالسعر: {result}");

                return Ok(new PredictionResponse
                {
                    DeviceName = specs.DeviceName,
                    PredictedPrice = result,
                    Currency = "USD",
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "حدث خطأ أثناء التنبؤ بالسعر");
            return StatusCode(500, new ErrorResponse
            {
                StatusCode = 500,
                Message = "Internal Server Error. Please try again later.",
                Detailed = ex.Message
            });
        }
    }
}

public class PhoneSpecs
{
    /// <summary>
    /// اسم الجهاز
    /// </summary>
    [Required]
    public string DeviceName { get; set; }

    /// <summary>
    /// إمكانية توسيع الذاكرة العشوائية
    /// </summary>
    public bool RamExpandable { get; set; }

    /// <summary>
    /// سعة البطارية (mAh)
    /// </summary>
    [Required]
    [Range(1000, 10000)]
    public int BatteryCapacity { get; set; }

    /// <summary>
    /// حجم الشاشة (بوصة)
    /// </summary>
    [Required]
    [Range(4.0, 8.0)]
    public double DisplaySize { get; set; }

    /// <summary>
    /// سعة التخزين (GB)
    /// </summary>
    [Required]
    [Range(32, 1024)]
    public int Storage { get; set; }

    /// <summary>
    /// الذاكرة العشوائية (GB)
    /// </summary>
    [Required]
    [Range(2, 16)]
    public int Ram { get; set; }

    /// <summary>
    /// معدل تحديث الشاشة (Hz)
    /// </summary>
    [Required]
    [Range(60, 144)]
    public int RefreshRate { get; set; }

    /// <summary>
    /// الكاميرا الأمامية
    /// </summary>
    [Required]
    [RegularExpression(@"^\d+MP$")]
    public string FrontCamera { get; set; }

    /// <summary>
    /// الكاميرا الخلفية
    /// </summary>
    [Required]
    [RegularExpression(@"^(\d+MP\s*\+\s*)*\d+MP$")]
    public string RearCamera { get; set; }

    /// <summary>
    /// سرعة الشحن (W)
    /// </summary>
    [Required]
    [Range(10, 200)]
    public int ChargingSpeed { get; set; }
}

public class PredictionResponse
{
    public string DeviceName { get; set; }
    public double PredictedPrice { get; set; }
    public string Currency { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Detailed { get; set; }
} 