using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutProcessETWListener
{
    public class Logging
    {
        public static ILoggerFactory LoggerFactory{ get => new LoggerFactory(); }
    }
}
