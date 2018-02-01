using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace MyStatelessService
{
    [EventSource(Name = "SGBIS-Event-Custom")]     // This is the name of my eventSource outside my program.  
    class CustomEvent : EventSource
    {               
        public void MyCustomMessage(string eventName, string message, DateTime when, int value) { WriteEvent(1, eventName, message, when, value); }

        // Typically you only create one EventSource and use it throughout your program.  Thus a static field makes sense.  
        public static CustomEvent Log = new CustomEvent();

    }
}
