using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWListener
{
    public interface IEventConsumer
    {
        void ProcessTraceEvent(TraceEvent value);

    }



}
