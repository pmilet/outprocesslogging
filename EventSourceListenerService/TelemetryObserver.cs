using Microsoft.ApplicationInsights;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWConsumer
{
   
    public class TelemetryObserver : IObserver<TraceEvent>
    {
        readonly IAIEventProcessor _eventProcessor;
        public TelemetryObserver(IAIEventProcessor processor)
        {
            _eventProcessor = processor;
        }

        public void OnCompleted()
        {
            Console.WriteLine("Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(TraceEvent value)
        {

            switch (value.EventName)
            {
                case "MetricValue":
                    _eventProcessor.TrackMetric(value);
                    break;
                case "RequestTime":
                    _eventProcessor.TrackRequest(value);
                    break;
                case "LogEvent":
                    _eventProcessor.TrackEvent(value);
                    break;
                default:
                    _eventProcessor.TrackTrace(value);
                    break;
            }
            Console.WriteLine(value.ProviderName);
        }

    
    }

}
