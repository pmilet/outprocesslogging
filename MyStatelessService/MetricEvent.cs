using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace MyStatelessService
{
    [EventSource(Name = "SGBIS-Metric-Time")]     // This is the name of my eventSource outside my program.  
    class MetricEvent : EventSource
    {
        [NonEvent]
        public void Value(double value)
        {
            MetricValue(value.ToString(), "Request-Time");
        }

        [Event(1, Level = EventLevel.Informational)]
        private void MetricValue(string value, string metricName)
        {
            WriteEvent(1, value, metricName);
        }

        public static MetricEvent Log = new MetricEvent();
    }
}
