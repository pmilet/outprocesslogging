using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Extensions.DependencyInjection;

namespace OutProcessETWConsumer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryContext, TelemetryContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ITelemetryContext telemetryContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new TelemetryFilter(next, telemetryContext));
            builder.Build();

            ListenProvider(telemetryContext, "SGBIS-Request","pmilet.DomainEvents","Microsoft.Extensions.Logging","SGBIS-Event-Custom", "SGBIS-Metric-Time");
        }


        private Task ListenProvider(ITelemetryContext context,params string[] providers)
        {
            using (var session = new TraceEventSession("MySession"))
            {
                foreach (string providerName in providers)
                {
                    session.EnableProvider(providerName);
                }
                var parser = new DynamicTraceEventParser(session.Source);
                var observer = new TelemetryObserver(new AIEventProcessor("062d261a-ce87-47b6-881e-ce4e4703c229",context));
                foreach (string providerName in providers)
                {
                    var eventStream = parser.Observe(providerName, null);
                    eventStream.Subscribe(observer);
                }

                session.Source.Process();   // Listen (forever) for events
            }
            return Task.CompletedTask;
        }
    }
}
