namespace HealthMonitor.Dashboard.ViewModels
{
    public class CovidCaseViewModel
    {
        public int Id { get; set; }
        public int TotalTestResults { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public string TotalCasesFormatted { get; set; } 
        public string HospitalizationRateFormatted { get; set; } 
        public string HospitalizedCurrently { get; set; }
        public int? Negative { get; set; }
        public int? Positive { get; set; }
        public double? HospitalizationRate { get; set; }
    }
}
