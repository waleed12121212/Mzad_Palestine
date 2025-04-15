using System.Text.Json;
using Mzad_Palestine_Core.DTO_s.Laptop;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Threading.Tasks;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Mzad_Palestine_Core.Models.ML;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class LaptopPredictionService : ILaptopPredictionService
    {
        private readonly string _modelPath;
        private readonly string _columnsPath;
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private JsonDocument _columnsMetadata;
        private bool _isModelTrained;

        public LaptopPredictionService()
        {
            _modelPath = Path.Combine(Directory.GetCurrentDirectory(), "ML", "laptop_price_model.zip");
            _columnsPath = Path.Combine(Directory.GetCurrentDirectory(), "ML", "columns.json");
            _mlContext = new MLContext();
            LoadColumnsMetadata(); // Load metadata first
            LoadModel(); // Then try to load model
        }

        private void LoadModel()
        {
            if (!File.Exists(_modelPath))
            {
                // If model doesn't exist, create a basic one with sample data
                var trainer = new ML.ModelTrainer();
                var trainingData = CreateSampleTrainingData();
                trainer.TrainAndSaveModel(trainingData);
                _isModelTrained = true;
            }

            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read))
            {
                _model = _mlContext.Model.Load(stream, out var schema);
                _isModelTrained = true;
            }
        }

        private List<LaptopPriceInputModel> CreateSampleTrainingData()
        {
            return new List<LaptopPriceInputModel>
            {
                new LaptopPriceInputModel
                {
                    Features = new float[] { 1, 2.5f, 15.6f, 512, 0, 16, 0, 0, 0, 0 },
                    Price = 999.99f
                },
                new LaptopPriceInputModel
                {
                    Features = new float[] { 1, 3.0f, 14.0f, 256, 1000, 8, 1, 1, 0, 1 },
                    Price = 799.99f
                },
                new LaptopPriceInputModel
                {
                    Features = new float[] { 0, 2.8f, 13.3f, 1000, 0, 32, 2, 2, 0, 2 },
                    Price = 1499.99f
                }
            };
        }

        private void LoadColumnsMetadata()
        {
            if (!File.Exists(_columnsPath))
            {
                throw new FileNotFoundException("Columns metadata file not found. Please ensure columns.json is present in the ML directory.");
            }

            var jsonString = File.ReadAllText(_columnsPath);
            _columnsMetadata = JsonDocument.Parse(jsonString);
        }

        public async Task<double> PredictPrice(LaptopPredictionRequestDto request)
        {
            if (!_isModelTrained)
            {
                throw new InvalidOperationException("Model is not trained yet.");
            }

            try
            {
                // Create prediction engine
                var predEngine = _mlContext.Model.CreatePredictionEngine<LaptopPriceInputModel, LaptopPricePrediction>(_model);

                // Get arrays of strings for each category
                var brands = _columnsMetadata.RootElement.GetProperty("Brand_columns").EnumerateArray().Select(x => x.GetString()).ToArray();
                var processors = _columnsMetadata.RootElement.GetProperty("Processor_Name_columns").EnumerateArray().Select(x => x.GetString()).ToArray();
                var displayTypes = _columnsMetadata.RootElement.GetProperty("Display_type_columns").EnumerateArray().Select(x => x.GetString()).ToArray();
                var gpus = _columnsMetadata.RootElement.GetProperty("GPU_columns").EnumerateArray().Select(x => x.GetString()).ToArray();

                // Find indices
                var brandIndex = Array.IndexOf(brands, request.Brand?.ToLower());
                var processorIndex = Array.IndexOf(processors, request.ProcessorName?.ToLower());
                var displayTypeIndex = Array.IndexOf(displayTypes, request.DisplayType?.ToLower());
                var gpuIndex = Array.IndexOf(gpus, request.GPU?.ToLower());

                // Convert request to input data with fixed-size feature vector
                var inputData = new LaptopPriceInputModel
                {
                    Features = new float[]
                    {
                        request.RamExpandable ? 1 : 0,
                        request.ProcessorSpeed,
                        request.DisplaySize,
                        request.SSDSize,
                        request.HDDSize,
                        request.RAMSize,
                        brandIndex >= 0 ? brandIndex : 0,
                        processorIndex >= 0 ? processorIndex : 0,
                        displayTypeIndex >= 0 ? displayTypeIndex : 0,
                        gpuIndex >= 0 ? gpuIndex : 0
                    }
                };

                // Make prediction
                var prediction = predEngine.Predict(inputData);
                return prediction.Price;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error predicting laptop price: {ex.Message}");
            }
        }

        public async Task<object> GetPredictionMetadata()
        {
            return _columnsMetadata.RootElement;
        }
    }
}
