using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// Added in addition to default, above
using System.Net.Http;
using dotnet_webapi_otel_appd.Models;

namespace dotnet_webapi_otel_appd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // Adding HTTP Client for example downstream call
        private static HttpClient httpClient = new HttpClient();

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            // Sending example HTTP call to see generated TraceId, SpanId in logs
            var res = httpClient.GetStringAsync("http://google.com").Result;

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
