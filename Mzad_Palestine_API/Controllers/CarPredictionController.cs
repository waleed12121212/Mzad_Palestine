using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarPredictionController : ControllerBase
    {
        private readonly ICarPricePredictionService _predictionService;

        public CarPredictionController(ICarPricePredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost("predict")]
        public async Task<ActionResult<double>> PredictCarPrice([FromBody] CarPredictionModel car)
        {
            try
            {
                var predictedPrice = await _predictionService.PredictPrice(car);
                return Ok(predictedPrice);
            }
            catch (Exception ex)
            {
                return BadRequest($"خطأ في التنبؤ بالسعر: {ex.Message}");
            }
        }
    }
} 