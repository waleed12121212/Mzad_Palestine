using System.Collections.Generic;

namespace Mzad_Palestine_Core.Models.ML
{
    public class PredictionMetadata
    {
        public List<string> Brands { get; set; }
        public List<string> ProcessorNames { get; set; }
        public List<string> DisplayTypes { get; set; }
        public List<string> CameraTypes { get; set; }
    }
} 