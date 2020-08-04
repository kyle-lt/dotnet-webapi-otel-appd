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

// OpenTelemetry Refs
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Samplers;

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

            // Add OpenTelemetry Console Exporter & Jaeger Exporter
            services.AddOpenTelemetry((builder) => builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .UseConsoleExporter()
                .UseJaegerExporter(jaeger =>
                {
                    jaeger.ServiceName = this.Configuration.GetValue<string>("Jaeger:ServiceName");
                    jaeger.AgentHost = this.Configuration.GetValue<string>("Jaeger:Host");
                    jaeger.AgentPort = this.Configuration.GetValue<int>("Jaeger:Port");
                })
                .SetSampler(new AlwaysOnSampler())
                );
            
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
