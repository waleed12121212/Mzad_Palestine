using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mzad_Palestine_API.ML
{
    public class PhoneModelTrainer
    {
        private readonly MLContext _mlContext;
        private readonly string _modelPath;
        private readonly string _modelDirectory;

        public PhoneModelTrainer()
        {
            _mlContext = new MLContext(seed: 0);
            _modelDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ML");
            _modelPath = Path.Combine(_modelDirectory, "phone_model.zip");

            // التأكد من وجود المجلد
            if (!Directory.Exists(_modelDirectory))
            {
                Directory.CreateDirectory(_modelDirectory);
            }
        }

        public void TrainAndSaveModel()
        {
            try
            {
                // إنشاء بيانات تدريب
                var trainingData = GetSampleTrainingData();

                // تحويل البيانات إلى IDataView
                var trainingDataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // تعريف خطوات معالجة البيانات
                var pipeline = _mlContext.Transforms.Concatenate("Features",
                    "RAM_Expandable",
                    "Battery_Capacity",
                    "Display_Size",
                    "Storage",
                    "RAM",
                    "Refresh_Rate",
                    "Charging_Speed")
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.Regression.Trainers.LbfgsPoissonRegression());

                // تدريب النموذج
                var model = pipeline.Fit(trainingDataView);

                // حفظ النموذج
                using (var fs = File.Create(_modelPath))
                {
                    _mlContext.Model.Save(model, trainingDataView.Schema, fs);
                }

                Console.WriteLine($"تم حفظ النموذج في: {_modelPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"حدث خطأ أثناء تدريب وحفظ النموذج: {ex.Message}");
                throw;
            }
        }

        private IEnumerable<PhoneMLModel> GetSampleTrainingData()
        {
            return new List<PhoneMLModel>
            {
                // Samsung Galaxy S23 Series
                new PhoneMLModel
                {
                    Device_Name = "Samsung Galaxy S23",
                    RAM_Expandable = 0,
                    Battery_Capacity = 3900,
                    Display_Size = 6.1f,
                    Storage = 128,
                    RAM = 8,
                    Refresh_Rate = 120,
                    Front_Camera = "12 MP",
                    Rear_Camera = "50 MP + 12 MP + 10 MP",
                    Charging_Speed = 25,
                    Price = 3349
                },
                new PhoneMLModel
                {
                    Device_Name = "Samsung Galaxy S23+",
                    RAM_Expandable = 0,
                    Battery_Capacity = 4700,
                    Display_Size = 6.6f,
                    Storage = 256,
                    RAM = 8,
                    Refresh_Rate = 120,
                    Front_Camera = "12 MP",
                    Rear_Camera = "50 MP + 12 MP + 10 MP",
                    Charging_Speed = 45,
                    Price = 4200
                },
                new PhoneMLModel
                {
                    Device_Name = "Samsung Galaxy S23 Ultra",
                    RAM_Expandable = 0,
                    Battery_Capacity = 5000,
                    Display_Size = 6.8f,
                    Storage = 256,
                    RAM = 12,
                    Refresh_Rate = 120,
                    Front_Camera = "12 MP",
                    Rear_Camera = "200 MP + 12 MP + 10 MP + 10 MP",
                    Charging_Speed = 45,
                    Price = 5100
                },

                // Samsung A Series
                new PhoneMLModel
                {
                    Device_Name = "Samsung Galaxy A54",
                    RAM_Expandable = 1,
                    Battery_Capacity = 5000,
                    Display_Size = 6.4f,
                    Storage = 128,
                    RAM = 8,
                    Refresh_Rate = 120,
                    Front_Camera = "32 MP",
                    Rear_Camera = "50 MP + 12 MP + 5 MP",
                    Charging_Speed = 25,
                    Price = 1700
                },
                new PhoneMLModel
                {
                    Device_Name = "Samsung Galaxy A34",
                    RAM_Expandable = 1,
                    Battery_Capacity = 5000,
                    Display_Size = 6.6f,
                    Storage = 128,
                    RAM = 6,
                    Refresh_Rate = 120,
                    Front_Camera = "13 MP",
                    Rear_Camera = "48 MP + 8 MP + 5 MP",
                    Charging_Speed = 25,
                    Price = 1300
                },

                // iPhone Series
                new PhoneMLModel
                {
                    Device_Name = "iPhone 15 Pro Max",
                    RAM_Expandable = 0,
                    Battery_Capacity = 4441,
                    Display_Size = 6.7f,
                    Storage = 256,
                    RAM = 8,
                    Refresh_Rate = 120,
                    Front_Camera = "12 MP",
                    Rear_Camera = "48 MP + 12 MP + 12 MP",
                    Charging_Speed = 20,
                    Price = 5500
                },
                new PhoneMLModel
                {
                    Device_Name = "iPhone 15",
                    RAM_Expandable = 0,
                    Battery_Capacity = 3349,
                    Display_Size = 6.1f,
                    Storage = 128,
                    RAM = 6,
                    Refresh_Rate = 60,
                    Front_Camera = "12 MP",
                    Rear_Camera = "48 MP + 12 MP",
                    Charging_Speed = 20,
                    Price = 3700
                }
            };
        }
    }

    public class PhoneMLModel
    {
        [LoadColumn(0)]
        public float RAM_Expandable { get; set; }

        [LoadColumn(1)]
        public float Battery_Capacity { get; set; }

        [LoadColumn(2)]
        public float Display_Size { get; set; }

        [LoadColumn(3)]
        public float Storage { get; set; }

        [LoadColumn(4)]
        public float RAM { get; set; }

        [LoadColumn(5)]
        public float Refresh_Rate { get; set; }

        [LoadColumn(6)]
        public string Front_Camera { get; set; }

        [LoadColumn(7)]
        public string Rear_Camera { get; set; }

        [LoadColumn(8)]
        public float Charging_Speed { get; set; }

        [LoadColumn(9)]
        public float Price { get; set; }

        [LoadColumn(10)]
        public string Device_Name { get; set; }
    }
} 