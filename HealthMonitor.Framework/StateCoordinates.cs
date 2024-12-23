using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitor.Framework
{
    public static class StateCoordinates
    {
        public static Dictionary<string, (double Latitude, double Longitude)> GetCoordinates() => new()
        {
            { "CA", (37.7749, -122.4194) },
            { "NY", (40.7128, -74.0060) },
            { "TX", (31.9686, -99.9018) },
            { "FL", (27.9944, -81.7603) },
            { "WA", (47.7511, -120.7401) },
            // Add all states with their lat/long coordinates
        };
    }
}
