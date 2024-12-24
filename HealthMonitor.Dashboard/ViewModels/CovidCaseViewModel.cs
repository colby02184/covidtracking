using System.ComponentModel.DataAnnotations;

namespace HealthMonitor.Dashboard.ViewModels
{
    public class CovidCaseViewModel
    {
        public int Id { get; set; }
        public int TotalTestResults { get; set; }

        [Required(ErrorMessage = "State abbreviation is required.")]
        [StringLength(maximumLength:2, ErrorMessage = "State abbreviation must be 2 characters.")]
        public string State { get; set; }

        public DateTime Date { get; set; }
        public string TotalCasesFormatted { get; set; } 
        public string HospitalizationRateFormatted { get; set; } 
        public string HospitalizedCurrently { get; set; }
        public int? Negative { get; set; }
        public int? Positive { get; set; }

        [MaxLength]
        public double? HospitalizationRate { get; set; }
        public double? Latitude { get; set; } // for heatmap
        public double? Longitude { get; set; } // for heatmap
    }
}
