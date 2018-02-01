using Microsoft.ApplicationInsights;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWListener
{
   
    public class EventObserver : IObserver<TraceEvent>
    {
        readonly IEventConsumer _eventProcessor;
        private readonly ILogger _logger;

        public EventObserver(IEventConsumer processor)
        {
            _eventProcessor = processor;
            _logger = Logging.LoggerFactory.CreateLogger<EventObserver>();
        }

        public void OnCompleted()
        {
            _logger.LogInformation("Completed");
        }

        public void OnError(Exception error)
        {
            _logger.LogError(error.Message);
        }

        public void OnNext(TraceEvent value)
        {
            _eventProcessor.ProcessTraceEvent(value);
            _logger.LogTrace($"trace processed from {value.ProviderName}");
        }

    
    }

}
