using Mzad_Palestine_Core.Models;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IPhonePredictionService
    {
        Task<double> PredictPrice(Phone phone);
        Task<Phone> SavePhoneSpecs(Phone phone);
    }
} 