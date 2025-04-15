using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using Mzad_Palestine_Core.Models.ML;

namespace Mzad_Palestine_Infrastructure.ML
{
    public class ModelTrainer
    {
        private readonly MLContext _mlContext;
        private readonly string _modelPath;

        public ModelTrainer()
        {
            _mlContext = new MLContext(seed: 0);
            _modelPath = Path.Combine(Directory.GetCurrentDirectory(), "ML", "laptop_price_model.zip");
        }

        public void TrainAndSaveModel(List<LaptopPriceInputModel> trainingData)
        {
            // Convert training data to IDataView
            var trainingDataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Create pipeline
            var pipeline = _mlContext.Transforms.NormalizeMinMax("Features")
                .Append(_mlContext.Regression.Trainers.Sdca(
                    labelColumnName: "Price",
                    featureColumnName: "Features",
                    maximumNumberOfIterations: 100));

            // Train the model
            var model = pipeline.Fit(trainingDataView);

            // Save the model
            _mlContext.Model.Save(model, trainingDataView.Schema, _modelPath);
        }
    }
}
