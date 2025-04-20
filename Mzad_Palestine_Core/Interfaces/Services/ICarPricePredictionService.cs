using Mzad_Palestine_Core.Models;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ICarPricePredictionService
    {
        Task<double> PredictPrice(CarPredictionModel car);
    }
} 