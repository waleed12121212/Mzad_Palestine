using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Laptop
{
    public class LaptopPredictionRequestDto
    {
        [Required]
        public string Brand { get; set; }

        [Required]
        public string ProcessorName { get; set; }

        [Required]
        public string DisplayType { get; set; }

        [Required]
        public string GPU { get; set; }

        [Required]
        public bool RamExpandable { get; set; }

        [Required]
        public float ProcessorSpeed { get; set; } // GHz

        [Required]
        public float DisplaySize { get; set; }

        [Required]
        public int SSDSize { get; set; }

        [Required]
        public int HDDSize { get; set; }

        [Required]
        public int RAMSize { get; set; }
    }
}
