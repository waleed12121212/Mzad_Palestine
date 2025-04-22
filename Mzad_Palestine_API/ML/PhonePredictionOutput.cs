using Microsoft.ML.Data;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class PhonePredictionOutput
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
} 