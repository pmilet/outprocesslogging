using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStatelessService.Controllers
{
    public class ValueRequestedDomainEventHandler : 
        HandleDomainEventsBase<ValueRequested>
    {
        public ValueRequestedDomainEventHandler(IDomainEventDispatcher domainEventDispatcher):base(domainEventDispatcher)
        { }

        public override void HandleEvent(ValueRequested domainEvent)
        {
            CustomEvent.Log.LogEvent( domainEvent.AggregateSource, domainEvent.Message, DateTime.Now, 0);
        }
    }
}
