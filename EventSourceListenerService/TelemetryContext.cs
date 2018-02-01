using System;

namespace OutProcessETWConsumer
{
    internal class TelemetryContext : ITelemetryContext
    {
        public Guid OperationId { get; set; }
    }
}