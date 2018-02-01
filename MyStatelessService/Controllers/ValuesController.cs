using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pmilet.DomainEvents;
using pmilet.Playback;
using pmilet.Playback.Core;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyStatelessService.Controllers
{
    [Route("api/[controller]")]
    //PM: MODIFIED
    public class ValuesController : Controller
    {
        ILogger _logger;
        IDomainEventDispatcher _domainEventDispatcher;
        IPlaybackContext _playback;
        public ValuesController( IDomainEventDispatcher domainEventDispatcher, IPlaybackContext playback)
        {
            _logger = Logging.LoggerFactory.CreateLogger<ValuesController>();
            _domainEventDispatcher = domainEventDispatcher;
            _playback = playback;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            RequestEvent.Log.Request("Get", true, 100);
            _domainEventDispatcher.Publish<ValueRequested>(new ValueRequested());
            CustomEvent.Log.MyCustomMessage("Get all", "call from Get() valuescontroller (semantic log)", DateTime.Now, 0);
            var name = "ValuesController";
            _logger.LogInformation("This is a classic trace message from {name}",name);

            MetricEvent.Log.Value(200);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [SwaggerOperationFilter(typeof(PlaybackSwaggerFilter))]
        public string Get(int id)
        {
            if (_playback.Fake?.ToLower() == "outbound")
            {
                return "fake value";
            }

            CustomEvent.Log.MyCustomMessage("Get id", "Get(id)", DateTime.Now, 0);
            _logger.LogInformation("This is a classic trace message");
            return $"value {id}";
        }

        // POST api/values
        [HttpPost]
        [SwaggerOperationFilter(typeof(PlaybackSwaggerFilter))]
        public void Post(string value)
        {
            _logger.LogInformation("Post() method called with {value}", value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
            _logger.LogInformation("Put() method called");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogError("not allowed for resource {id}",id);
        }
    }
}
