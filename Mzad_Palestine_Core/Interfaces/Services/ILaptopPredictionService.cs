using Mzad_Palestine_Core.DTO_s.Laptop;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface ILaptopPredictionService
    {
        Task<double> PredictPrice(LaptopPredictionRequestDto request);
        Task<object> GetPredictionMetadata();
    }
}
