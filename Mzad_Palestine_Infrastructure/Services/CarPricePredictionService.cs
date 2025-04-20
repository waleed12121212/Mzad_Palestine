using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;
using Microsoft.ML.Data;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class CarPricePredictionService : ICarPricePredictionService
    {
        private readonly IHostEnvironment _hostEnvironment;

        public CarPricePredictionService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<double> PredictPrice(CarPredictionModel car)
        {
            try
            {
                // السعر الأساسي للسيارة
                double basePrice = 25000;

                // عوامل التأثير على السعر
                double horsepowerFactor = car.Horsepower ?? 0 * 150;  // كل حصان يزيد 150 دولار
                double weightFactor = (car.CurbWeight ?? 0) * 0.5;    // كل كيلو يزيد 0.5 دولار
                double engineSizeFactor = (car.EngineSize ?? 0) * 5;  // كل سي سي يزيد 5 دولار

                // عامل العلامة التجارية
                double brandFactor = (car.Brand?.ToLower()) switch
                {
                    "bmw" => 25000,
                    "mercedes" => 25000,
                    "audi" => 20000,
                    "lexus" => 18000,
                    "porsche" => 35000,
                    "toyota" => 8000,
                    "honda" => 7000,
                    "volkswagen" => 12000,
                    "ford" => 10000,
                    "chevrolet" => 9000,
                    "hyundai" => 6000,
                    "kia" => 5000,
                    _ => 5000
                };

                // عامل نوع الجسم
                double bodyFactor = (car.CarBody?.ToLower()) switch
                {
                    "coupe" => 8000,
                    "convertible" => 12000,
                    "suv" => 10000,
                    "wagon" => 6000,
                    "sedan" => 5000,
                    "hatchback" => 4000,
                    _ => 4000
                };

                // عامل نوع المحرك
                double engineTypeFactor = (car.EngineType?.ToLower()) switch
                {
                    "dohc" => 5000,
                    "sohc" => 3000,
                    "ohc" => 2500,
                    "ohv" => 2000,
                    "rotor" => 6000,
                    _ => 2000
                };

                // عامل عدد الاسطوانات
                double cylinderFactor = (car.CylinderNumber?.ToLower()) switch
                {
                    "twelve" or "12" => 15000,
                    "eight" or "8" => 10000,
                    "six" or "6" => 6000,
                    "five" or "5" => 4000,
                    "four" or "4" => 3000,
                    "three" or "3" => 2000,
                    "two" or "2" => 1000,
                    _ => 2000
                };

                // حساب السعر الأساسي
                double predictedPrice = basePrice + horsepowerFactor + weightFactor + engineSizeFactor + 
                                     brandFactor + bodyFactor + engineTypeFactor + cylinderFactor;

                // العوامل المضاعفة
                if (car.Aspiration?.ToLower() == "turbo")
                    predictedPrice *= 1.2;  // زيادة 20% للتيربو

                if (car.FuelType?.ToLower() == "diesel")
                    predictedPrice *= 1.1;  // زيادة 10% للديزل

                if (car.DriveWheel?.ToLower() == "awd")
                    predictedPrice *= 1.15; // زيادة 15% للدفع الرباعي

                // عوامل الكفاءة
                double efficiencyFactor = ((car.CityMPG ?? 0) + (car.HighwayMPG ?? 0)) / 2;
                if (efficiencyFactor > 30)
                    predictedPrice *= 1.1;  // زيادة 10% للسيارات الموفرة للوقود

                // تعديل السعر حسب سنة الصنع (إذا كانت متوفرة)
                // يمكن إضافة المزيد من العوامل هنا

                return await Task.FromResult(Math.Round(predictedPrice, 2));
            }
            catch (Exception ex)
            {
                throw new Exception($"حدث خطأ أثناء التنبؤ بالسعر: {ex.Message}");
            }
        }
    }

    internal class CarPredictionInput
    {
        [ColumnName("symboling")]
        public float Symboling { get; set; }

        [ColumnName("fueltype")]
        public string FuelType { get; set; }

        [ColumnName("aspiration")]
        public string Aspiration { get; set; }

        [ColumnName("doornumber")]
        public string DoorNumber { get; set; }

        [ColumnName("carbody")]
        public string CarBody { get; set; }

        [ColumnName("drivewheel")]
        public string DriveWheel { get; set; }

        [ColumnName("enginelocation")]
        public string EngineLocation { get; set; }

        [ColumnName("wheelbase")]
        public float WheelBase { get; set; }

        [ColumnName("carlength")]
        public float CarLength { get; set; }

        [ColumnName("carwidth")]
        public float CarWidth { get; set; }

        [ColumnName("carheight")]
        public float CarHeight { get; set; }

        [ColumnName("curbweight")]
        public float CurbWeight { get; set; }

        [ColumnName("enginetype")]
        public string EngineType { get; set; }

        [ColumnName("cylindernumber")]
        public string CylinderNumber { get; set; }

        [ColumnName("enginesize")]
        public float EngineSize { get; set; }

        [ColumnName("fuelsystem")]
        public string FuelSystem { get; set; }

        [ColumnName("boreratio")]
        public float BoreRatio { get; set; }

        [ColumnName("stroke")]
        public float Stroke { get; set; }

        [ColumnName("compressionratio")]
        public float CompressionRatio { get; set; }

        [ColumnName("horsepower")]
        public float Horsepower { get; set; }

        [ColumnName("peakrpm")]
        public float PeakRPM { get; set; }

        [ColumnName("citympg")]
        public float CityMPG { get; set; }

        [ColumnName("highwaympg")]
        public float HighwayMPG { get; set; }

        [ColumnName("brand")]
        public string Brand { get; set; }

        [ColumnName("model")]
        public string Model { get; set; }
    }

    internal class CarPredictionOutput
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
} 