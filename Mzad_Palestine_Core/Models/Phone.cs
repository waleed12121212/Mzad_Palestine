using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.Models
{
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }

        [Required]
        public string DeviceName { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
} 