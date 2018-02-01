using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWListener
{
    public class EventListenerProcess
    {
        public static Task Process(IEventConsumer consumer, params string[] providers)
        {
            using (var session = new TraceEventSession("MySession"))
            {
                foreach (string providerName in providers)
                {
                    session.EnableProvider(providerName);
                }
                var parser = new DynamicTraceEventParser(session.Source);
                var observer = new EventObserver(consumer);
                foreach (string providerName in providers)
                {
                    var eventStream = parser.Observe(providerName, null);
                    eventStream.Subscribe(observer);
                }

                session.Source.Process();   // Listen (forever) for events
            }
            return Task.CompletedTask;
        }
    }
}
