using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace MyStatelessService
{
    public class TransactionTelemetryFilter : ITelemetryProcessor
    {
        private readonly ITransactionTelemetryContext _context;
        // Link processors to each other in a chain.
        public TransactionTelemetryFilter(ITelemetryProcessor processor, ITransactionTelemetryContext context)
        {
            Next = processor;
            _context = context;
        }
     
        private ITelemetryProcessor Next { get; set; }
       
        public void Process(ITelemetry item)
        {
            if (IsFromSyntheticSource(item))
            { return; }

            if (IsFromSwagger(item))
            { return; }

            if(IsHealthRequest(item))
            { return; }

            if( !IsTrace(item) && !IsRequest(item) && !IsEvent(item, "pmilet") && !IsEvent(item) && !IsMetric(item) && !IsDependency(item))
            {
                return;
            }

            //PM TODO: Revisar la forma correcta de correlacionar
            item.Context.Operation.Id = _context.OperationId.ToString();

            this.Next.Process(item);

        }

        private bool IsTrace(ITelemetry item)
        {
            var trace = item as TraceTelemetry;
            if (trace != null)
                return true;
            return false;
        }

        private bool IsDependency(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency != null)
                return true;
            return false;
        }

        private bool IsMetric(ITelemetry item, string providerText = "sgbis")
        {
            var metric = item as MetricTelemetry;
            if (metric == null || metric.Name == null || !metric.Properties.ContainsKey("ProviderName"))
                return false;

            if (metric.Properties["ProviderName"].ToLower().Contains(providerText))
                return true;
            return false;

        }

        private bool IsEvent(ITelemetry item, string providerText="sgbis")
        {
            var switchEvent = item as EventTelemetry;
            if (switchEvent == null || switchEvent.Name == null || !switchEvent.Properties.ContainsKey("ProviderName"))
                return false;

            if (switchEvent.Properties["ProviderName"].ToLower().Contains(providerText))
                return true;

            return false;
        }

        private bool IsRequest(ITelemetry item)
        {
            var request = item as RequestTelemetry;
            if (request != null)
                return true;
            return false;
        }

        private bool IsHealthRequest(ITelemetry item)
        {
            var request = item as RequestTelemetry;
            if (request == null) return false;

            if (item.Context.Operation.Name == "GET /health")
            {
                return true;
            }
            return false;
        }

        private bool IsFromSwagger(ITelemetry item)
        {
            if (!string.IsNullOrEmpty(item.Context.Operation.Name))
            {
                if (item.Context.Operation.Name.ToLower().Contains("swagger"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsFromSyntheticSource(ITelemetry item)
        {
            return !string.IsNullOrEmpty(item.Context.Operation.SyntheticSource);
        }
    }
}
