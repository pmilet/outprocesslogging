using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyStatelessService.Controllers;
using pmilet.DomainEvents;
using pmilet.Playback;

namespace MyStatelessService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddSingleton<ITransactionTelemetryContext, TransactionTelemetryContext>();
            services.AddScoped<ValueRequestedDomainEventHandler, ValueRequestedDomainEventHandler>();

            services.AddElm(options =>
            {
                options.Path = new Microsoft.AspNetCore.Http.PathString("/elm");
                options.Filter = (a, logLevel) => logLevel >= Microsoft.Extensions.Logging.LogLevel.Information;
            });

            services.AddPlayback(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Test API", Version = "v1" });
            });

         
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ValueRequestedDomainEventHandler handler)
        {
            loggerFactory.AddEventSourceLogger();
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1"));
            app.UseElmPage();
            app.UseElmCapture();

            app.UsePlayback();
            
            app.UseMiddleware<TelemetryMiddleware>();
            app.UseMvc();

        }        

    }
}
