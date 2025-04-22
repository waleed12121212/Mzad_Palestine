using Microsoft.ML.Data;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class PhoneMLModel
    {
        [ColumnName("RAM_Expandable")]
        public float RAM_Expandable { get; set; }

        [ColumnName("Battery_Capacity")]
        public float Battery_Capacity { get; set; }

        [ColumnName("Display_Size")]
        public float Display_Size { get; set; }

        [ColumnName("Storage")]
        public float Storage { get; set; }

        [ColumnName("RAM")]
        public float RAM { get; set; }

        [ColumnName("Refresh_Rate")]
        public float Refresh_Rate { get; set; }

        [ColumnName("Front_Camera")]
        public string Front_Camera { get; set; }

        [ColumnName("Rear_Camera")]
        public string Rear_Camera { get; set; }

        [ColumnName("Charging_Speed")]
        public float Charging_Speed { get; set; }

        [ColumnName("Price")]
        public float Price { get; set; }
    }
} 