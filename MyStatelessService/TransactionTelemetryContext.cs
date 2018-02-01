using System;

namespace MyStatelessService
{
    internal class TransactionTelemetryContext : ITransactionTelemetryContext
    {
        public Guid OperationId { get; set; }
    }
}