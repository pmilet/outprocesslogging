using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWConsumer
{
    public class AIEventProcessor : IAIEventProcessor
    {
        private TelemetryClient telemetryClient = new TelemetryClient();
        private readonly ITelemetryContext _context;

        public AIEventProcessor(string instrumentationKey, ITelemetryContext context)
        {
            telemetryClient.InstrumentationKey = instrumentationKey;
            _context = context;
        }

        public void TrackRequest(TraceEvent value)
        {
            _context.OperationId = Guid.NewGuid();
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            telemetryClient.TrackRequest(value.EventName, DateTime.Now, new TimeSpan(0,0,1), "200", true);
        }

        public void TrackEvent(TraceEvent value)
        {
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var name in value.PayloadNames)
            {
                properties.Add(name, value.PayloadStringByName(name));
            }
            telemetryClient.TrackEvent(value.EventName, properties);
        }

        public void TrackTrace(TraceEvent value)
        {
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var name in value.PayloadNames)
            {
                properties.Add(name, value.PayloadStringByName(name));
            }
            telemetryClient.TrackTrace(value.FormattedMessage, properties);
        }

        public void TrackMetric(TraceEvent value)
        {
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            MetricTelemetry telemetry = new MetricTelemetry(value.PayloadStringByName("metricName"), double.Parse(value.PayloadStringByName("value")));
            telemetryClient.TrackMetric(telemetry);
        }
    }
}
