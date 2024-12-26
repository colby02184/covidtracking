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
        public int? ProbableCases { get; set; }
        public int? Negative { get; set; }
        public int? Pending { get; set; }
        public string? TotalTestResultsSource { get; set; }
        public int? TotalTestResults { get; set; }
        public int? HospitalizedCurrently { get; set; }
        public int? HospitalizedCumulative { get; set; }
        public int? InIcuCurrently { get; set; }
        public int? InIcuCumulative { get; set; }
        public int? OnVentilatorCurrently { get; set; }
        public int? OnVentilatorCumulative { get; set; }
        public int? Recovered { get; set; }
        public string? LastUpdateEt { get; set; }
        public DateTime? DateModified { get; set; }
        public string? CheckTimeEt { get; set; }
        public int? Death { get; set; }
        public int? Hospitalized { get; set; }
        public int? HospitalizedDischarged { get; set; }
        public DateTime? DateChecked { get; set; }
        public int? TotalTestsViral { get; set; }
        public int? PositiveTestsViral { get; set; }
        public int? NegativeTestsViral { get; set; }
        public int? PositiveCasesViral { get; set; }
        public int? DeathConfirmed { get; set; }
        public int? DeathProbable { get; set; }
        public int? TotalTestEncountersViral { get; set; }
        public int? TotalTestsPeopleViral { get; set; }
        public int? TotalTestsAntibody { get; set; }
        public int? PositiveTestsAntibody { get; set; }
        public int? NegativeTestsAntibody { get; set; }
        public int? TotalTestsPeopleAntibody { get; set; }
        public int? PositiveTestsPeopleAntibody { get; set; }
        public int? NegativeTestsPeopleAntibody { get; set; }
        public int? TotalTestsPeopleAntigen { get; set; }
        public int? PositiveTestsPeopleAntigen { get; set; }
        public int? TotalTestsAntigen { get; set; }
        public int? PositiveTestsAntigen { get; set; }
        public string? FIPS { get; set; }
        public int? PositiveIncrease { get; set; }
        public int? NegativeIncrease { get; set; }
        public int? Total { get; set; }
        public int? TotalTestResultsIncrease { get; set; }
        public int? PosNeg { get; set; }
        public string? DataQualityGrade { get; set; }
        public int? DeathIncrease { get; set; }
        public int? HospitalizedIncrease { get; set; }
        public string? Hash { get; set; }
        public int? CommercialScore { get; set; }
        public int? NegativeRegularScore { get; set; }
        public int? NegativeScore { get; set; }
        public int? PositiveScore { get; set; }
        public int? Score { get; set; }
        public string? Grade { get; set; }
        public double? Latitude { get; set; } // for heatmap
        public double? Longitude { get; set; }// for heatmap
        public double? HospitalizationRate { get; set; }
    }
}
