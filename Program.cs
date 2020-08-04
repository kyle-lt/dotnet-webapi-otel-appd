using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace dotnet_webapi_otel_appd
{
    public class Program
    {
        static readonly ActivitySource activitySource = new ActivitySource(
        "dotnet_webapi_otel_appd.api.WeatherForecast");

        public static void Main(string[] args)
        {
            // Configure W3C Context Propagation
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            // Configure Trace Provider
            /*
            using var otel = Sdk.CreateTracerProvider(b => b
            .AddActivitySource("dotnet_webapi_otel_appd.api.WeatherForecast")
            .UseConsoleExporter());
            */
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole((options) => { options.IncludeScopes = true; });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
