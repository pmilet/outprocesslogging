using pmilet.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStatelessService
{
    public class ValueRequested : DomainEvent
    {
        public ValueRequested() : base("MyStatelessService", "1.0")
        { }

        public string Message { get; set; }
    }
}
