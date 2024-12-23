namespace HealthMonitor.Dashboard.ViewModels
{
    public class HeatmapDataViewModel
    {
        public string State { get; set; }
        public DateTime Date { get; set; } 
        public double? HospitalizationRate { get; set; } 
    }
}
