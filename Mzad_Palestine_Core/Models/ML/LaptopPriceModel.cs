using Microsoft.ML.Data;

namespace Mzad_Palestine_Core.Models.ML
{
    public class LaptopPriceInputModel
    {
        // Numeric features
        [VectorType(10)]
        [LoadColumn(0)]
        public float[] Features { get; set; }

        [LoadColumn(1)]
        public float Price { get; set; }
    }

    public class LaptopPricePrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}
