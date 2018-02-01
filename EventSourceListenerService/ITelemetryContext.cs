using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWConsumer
{
    public interface ITelemetryContext
    {
        Guid OperationId { get; set; }
    }
}
