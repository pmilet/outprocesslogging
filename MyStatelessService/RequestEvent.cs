using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace MyStatelessService
{
    [EventSource(Name = "SGBIS-Request")]     // This is the name of my eventSource outside my program.  
    class RequestEvent : EventSource
    {
        public void Request(string requestName, bool isSuccess, long durationMsec)
        { WriteEvent(1, requestName, isSuccess, durationMsec); }

        public static RequestEvent Log = new RequestEvent();
    }
}
