using System;

namespace OutProcessETWListener
{
    internal class TelemetryContext : ITelemetryContext
    {
        public Guid OperationId { get; set; }
    }
}