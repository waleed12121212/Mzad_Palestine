using System.ComponentModel.DataAnnotations;

namespace PhonePricePrediction.Models
{
    public class PhoneSpecs
    {
        [Required]
        public string DeviceName { get; set; }

        [Required]
        public bool RamExpandable { get; set; }

        [Required]
        [Range(1000, 10000)]
        public int BatteryCapacity { get; set; }

        [Required]
        [Range(4.0, 8.0)]
        public double DisplaySize { get; set; }

        [Required]
        [Range(32, 1024)]
        public int Storage { get; set; }

        [Required]
        [Range(2, 16)]
        public int Ram { get; set; }

        [Required]
        [Range(60, 144)]
        public int RefreshRate { get; set; }

        [Required]
        [RegularExpression(@"^\d+MP$")]
        public string FrontCamera { get; set; }

        [Required]
        [RegularExpression(@"^(\d+MP\s*\+\s*)*\d+MP$")]
        public string RearCamera { get; set; }

        [Required]
        [Range(10, 200)]
        public int ChargingSpeed { get; set; }
    }
} 