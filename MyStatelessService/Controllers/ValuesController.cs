using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pmilet.DomainEvents;

namespace MyStatelessService.Controllers
{
    [Route("api/[controller]")]
    //PM: MODIFIED
    public class ValuesController : Controller
    {
        ILogger _logger;
        IDomainEventDispatcher _domainEventDispatcher;
        public ValuesController( IDomainEventDispatcher domainEventDispatcher)
        {
            _logger = Logging.LoggerFactory.CreateLogger<ValuesController>();
            _domainEventDispatcher = domainEventDispatcher;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            RequestEvent.Log.Request("Get", true, 100);
            _domainEventDispatcher.Publish<ValueRequested>(new ValueRequested());
            CustomEvent.Log.LogEvent("Get all", "call from Get() valuescontroller (semantic log)", DateTime.Now, 0);
            var name = "ValuesController";
            _logger.LogInformation("This is a classic trace message from {name}",name);
            MetricEvent.Log.Value(200);
            return new string[] { "value1", "value2" };
        }
       
    }
}
