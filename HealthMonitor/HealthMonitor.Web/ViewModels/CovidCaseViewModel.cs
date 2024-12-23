namespace HealthMonitor.Web.ViewModels
{
    public class CovidCaseViewModel
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Date { get; set; } // Formatted as "MM/dd/yyyy"
        public string TotalCasesFormatted { get; set; } // Formatted for UI display
        public string HospitalizationRateFormatted { get; set; } // E.g., "12.5%"
    }
}
