using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Framework
{
    public class CovidSummary
    {
        public int? Positive { get; set; }
        public int? Negative { get; set; }
        public int? HospitalizedCurrently { get; set; }
        public int? Death { get; set; }
        public int? TotalTestResults { get; set; }
    }
}
