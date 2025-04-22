using Mzad_Palestine_Core.DTO_s.Phone;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IPhonePredictionService
    {
        Task<string> PredictPrice(PhonePredictionRequestDto request);
        Task<object> GetPredictionMetadata();
    }
} 