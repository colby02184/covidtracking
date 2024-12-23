namespace HealthMonitor.Dashboard.ViewModels
{
    public class CovidSummaryViewModel
    {
        public int? Positive { get; set; }
        public int? Negative { get; set; }
        public int? HospitalizedCurrently { get; set; }
        public int? Death { get; set; }
        public int? TotalTestResults { get; set; }
    }
}
