using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWListener
{
    public class AppInsightsEventConsumer : IEventConsumer
    {
        private TelemetryClient telemetryClient = new TelemetryClient();
        private readonly ITelemetryContext _context;

        public AppInsightsEventConsumer(IOptions<AppInsightsEventConsumerOptions> optionsAccesor, ITelemetryContext context)
        {
            telemetryClient.InstrumentationKey = optionsAccesor.Value.InstrumentationKey;
            _context = context;
            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new TelemetryFilter(next, context));
            builder.Build();

        }

        private void TrackRequest(TraceEvent value)
        {
            _context.OperationId = Guid.NewGuid();
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            telemetryClient.TrackRequest(value.EventName, DateTime.Now, new TimeSpan(0,0,1), "200", true);
        }

        private void TrackEvent(TraceEvent value)
        {
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var name in value.PayloadNames)
            {
                properties.Add(name, value.PayloadStringByName(name));
            }
            telemetryClient.TrackEvent(value.EventName, properties);
        }

        private void TrackTrace(TraceEvent value)
        {
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var name in value.PayloadNames)
            {
                properties.Add(name, value.PayloadStringByName(name));
            }
            telemetryClient.TrackTrace(value.FormattedMessage, properties);
        }

        private void TrackMetric(TraceEvent value)
        {
            telemetryClient.Context.Operation.Id = _context.OperationId.ToString();
            MetricTelemetry telemetry = new MetricTelemetry(value.PayloadStringByName("metricName"), double.Parse(value.PayloadStringByName("value")));
            telemetryClient.TrackMetric(telemetry);
        }

        public void ProcessTraceEvent(TraceEvent value)
        {
            switch (value.EventName)
            {
                case "MetricValue":
                    this.TrackMetric(value);
                    break;
                case "Request":
                    this.TrackRequest(value);
                    break;
                case "LogEvent":
                    this.TrackEvent(value);
                    break;
                default:
                    this.TrackTrace(value);
                    break;
            }
        }
    }
}
