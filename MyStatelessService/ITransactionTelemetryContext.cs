using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStatelessService
{
    public interface ITransactionTelemetryContext
    {
        Guid OperationId { get; set; }
    }
}
