using Microsoft.ML;
using Microsoft.Extensions.Caching.Memory;
using Mzad_Palestine_Core.DTO_s.Phone;
using Mzad_Palestine_Core.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class PhonePredictionService : IPhonePredictionService
    {
        private readonly MLContext _mlContext;
        private readonly string _modelPath;
        private readonly IMemoryCache _cache;
        private const string MODEL_CACHE_KEY = "PhonePredictionModel";
        private const string CURRENCY_SYMBOL = ""; // تم إزالة رمز الشيكل

        public PhonePredictionService(IMemoryCache cache)
        {
            _mlContext = new MLContext(seed: 0);
            _modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ML", "phone_model.zip");
            _cache = cache;
        }

        private ITransformer GetModel()
        {
            if (_cache.TryGetValue(MODEL_CACHE_KEY, out ITransformer model))
            {
                return model;
            }

            if (!File.Exists(_modelPath))
            {
                throw new FileNotFoundException("لم يتم العثور على ملف النموذج. يرجى التأكد من تدريب النموذج أولاً.", _modelPath);
            }

            using (var stream = File.OpenRead(_modelPath))
            {
                model = _mlContext.Model.Load(stream, out var _);
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1));
                
                _cache.Set(MODEL_CACHE_KEY, model, cacheOptions);
                
                return model;
            }
        }

        public async Task<string> PredictPrice(PhonePredictionRequestDto request)
        {
            try
            {
                var model = GetModel();
                var predictor = _mlContext.Model.CreatePredictionEngine<PhoneMLModel, PhonePricePrediction>(model);

                var mlModel = new PhoneMLModel
                {
                    Device_Name = request.DeviceName,
                    RAM_Expandable = request.RamExpandable ? 1 : 0,
                    Battery_Capacity = request.BatteryCapacity,
                    Display_Size = request.DisplaySize,
                    Storage = request.Storage,
                    RAM = request.Ram,
                    Refresh_Rate = request.RefreshRate,
                    Front_Camera = request.FrontCamera,
                    Rear_Camera = request.RearCamera,
                    Charging_Speed = request.ChargingSpeed
                };

                var prediction = predictor.Predict(mlModel);
                var formattedPrice = $"{prediction.Price:N0} {CURRENCY_SYMBOL}";
                return await Task.FromResult(formattedPrice);
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء التنبؤ بالسعر: {ex.Message}");
            }
        }

        public async Task<object> GetPredictionMetadata()
        {
            var metadata = new
            {
                ModelPath = _modelPath,
                IsModelExists = File.Exists(_modelPath),
                Currency = CURRENCY_SYMBOL,
                Features = new[]
                {
                    "Device_Name",
                    "RAM_Expandable",
                    "Battery_Capacity",
                    "Display_Size",
                    "Storage",
                    "RAM",
                    "Refresh_Rate",
                    "Front_Camera",
                    "Rear_Camera",
                    "Charging_Speed"
                }
            };

            return await Task.FromResult(metadata);
        }
    }

    public class PhoneMLModel
    {
        [Microsoft.ML.Data.LoadColumn(0)]
        public string Device_Name { get; set; }

        [Microsoft.ML.Data.LoadColumn(1)]
        public float RAM_Expandable { get; set; }

        [Microsoft.ML.Data.LoadColumn(2)]
        public float Battery_Capacity { get; set; }

        [Microsoft.ML.Data.LoadColumn(3)]
        public float Display_Size { get; set; }

        [Microsoft.ML.Data.LoadColumn(4)]
        public float Storage { get; set; }

        [Microsoft.ML.Data.LoadColumn(5)]
        public float RAM { get; set; }

        [Microsoft.ML.Data.LoadColumn(6)]
        public float Refresh_Rate { get; set; }

        [Microsoft.ML.Data.LoadColumn(7)]
        public string Front_Camera { get; set; }

        [Microsoft.ML.Data.LoadColumn(8)]
        public string Rear_Camera { get; set; }

        [Microsoft.ML.Data.LoadColumn(9)]
        public float Charging_Speed { get; set; }

        [Microsoft.ML.Data.LoadColumn(10)]
        public float Price { get; set; }
    }

    public class PhonePricePrediction
    {
        [Microsoft.ML.Data.ColumnName("Score")]
        public float Price { get; set; }
    }
} 