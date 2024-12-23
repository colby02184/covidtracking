using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthMonitor.Framework
{
    public class CovidData
    {
        public int Id { get; set; }
        [JsonConverter(typeof(DateConverter))]
        public DateTime Date { get; set; } 
        public string? State { get; set; } 
        public int? Positive { get; set; }
        public int? Negative { get; set; }
        public int? HospitalizedCurrently { get; set; }
        public int? Death { get; set; }
        public int? TotalTestResults { get; set; }
        public int? Recovered { get; set; }
        public string? DataQualityGrade { get; set; } 
        public string? FIPS { get; set; } 
        public string? LastUpdateEt { get; set; }
        public double? HospitalizationRate { get; set; }
        public double? Latitude { get; set; } // for heatmap
        public double? Longitude { get; set; } // for heatmap
    }
}
