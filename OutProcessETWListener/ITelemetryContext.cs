using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWListener
{
    public interface ITelemetryContext
    {
        Guid OperationId { get; set; }
    }
}
