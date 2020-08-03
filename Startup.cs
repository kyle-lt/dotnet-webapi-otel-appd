using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace dotnet_webapi_otel_appd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add OpenTelemetry Console Exporter
            services.AddOpenTelemetry((builder) => builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .UseConsoleExporter());
            
            // Add OpenTelemetry Jaeger Exporter
            // The below properties are defined in appsettings.json
            /*
            services.AddOpenTelemetry((builder) => builder
                .AddAspNetCoreInstrumentation()
                .AddHttpInstrumentation()
                .UseJaegerActivityExporter(o =>
                {
                    o.ServiceName = this.Configuration.GetValue<string>("Jaeger:ServiceName");
                    o.AgentHost = this.Configuration.GetValue<string>("Jaeger:Host");
                    o.AgentPort = this.Configuration.GetValue<int>("Jaeger:Port");
                }));
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
