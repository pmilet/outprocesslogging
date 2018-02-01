using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace MyStatelessService
{
    public class TelemetryMiddleware
    {
        protected readonly RequestDelegate _next;
        protected readonly ITransactionTelemetryContext _telemetryContext;

        public TelemetryMiddleware(RequestDelegate next, ITransactionTelemetryContext filter)
        {
            _next = next;
            _telemetryContext = filter;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _telemetryContext.OperationId = Guid.NewGuid();

            await _next.Invoke(httpContext);
        }
    }
}
