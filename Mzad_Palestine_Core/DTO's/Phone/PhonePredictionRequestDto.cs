using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTO_s.Phone
{
    public class PhonePredictionRequestDto
    {
        [Required]
        public string DeviceName { get; set; }

        [Required]
        public bool RamExpandable { get; set; }

        [Required]
        public float BatteryCapacity { get; set; }

        [Required]
        public float DisplaySize { get; set; }

        [Required]
        public float Storage { get; set; }

        [Required]
        public float Ram { get; set; }

        [Required]
        public float RefreshRate { get; set; }

        [Required]
        public string FrontCamera { get; set; }

        [Required]
        public string RearCamera { get; set; }

        [Required]
        public float ChargingSpeed { get; set; }
    }
} 