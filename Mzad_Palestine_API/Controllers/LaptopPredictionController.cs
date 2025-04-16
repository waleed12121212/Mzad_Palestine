using Microsoft.AspNetCore.Mvc;
using Mzad_Palestine_Core.DTO_s.Laptop;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Models.ML;
using System;
using System.Threading.Tasks;

namespace Mzad_Palestine_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LaptopPredictionController : ControllerBase
    {
        private readonly ILaptopPredictionService _predictionService;

        public LaptopPredictionController(ILaptopPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictPrice([FromBody] LaptopPredictionRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var predictedPrice = await _predictionService.PredictPrice(request);
                return Ok(new { PredictedPrice = predictedPrice });
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new
                {
                    StatusCode = 500 ,
                    Message = "Internal Server Error. Please try again later." ,
                    Detailed = ex.Message
                });
            }
        }

        [HttpGet("metadata")]
        public async Task<IActionResult> GetMetadata( )
        {
            try
            {
                var metadata = await _predictionService.GetPredictionMetadata();
                return Ok(metadata);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new
                {
                    StatusCode = 500 ,
                    Message = "Internal Server Error. Please try again later." ,
                    Detailed = ex.Message
                });
            }
        }
    }
}
