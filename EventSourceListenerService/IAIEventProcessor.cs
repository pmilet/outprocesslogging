using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWConsumer
{
    public interface IAIEventProcessor
    {
        void TrackEvent(TraceEvent value);

        void TrackTrace(TraceEvent value);
        void TrackRequest(TraceEvent value);
        void TrackMetric(TraceEvent value);
    }



}
