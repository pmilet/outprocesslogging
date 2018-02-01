using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace OutProcessETWListener
{
    public class TelemetryFilter : ITelemetryProcessor
    {
        private readonly ITelemetryContext _context;
        // Link processors to each other in a chain.
        public TelemetryFilter(ITelemetryProcessor processor, ITelemetryContext context)
        {
            Next = processor;
            _context = context;
        }
     
        private ITelemetryProcessor Next { get; set; }
       
        public void Process(ITelemetry item)
        {
            //PM TODO: Revisar la forma correcta de correlacionar

            item.Context.Operation.Id = _context.OperationId.ToString();

            this.Next.Process(item);

        }

    }
}
