using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using redis_demo_asp_net5.Attributes;
using redis_demo_asp_net5.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace redis_demo_asp_net5.Controllers
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

        private readonly IResponeCacheService _responeCacheService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponeCacheService responeCacheService)
        {
            _logger = logger;
            _responeCacheService = responeCacheService;
        }

        [HttpGet("getall")]
        [Cache(1000)]
        public async Task<IActionResult> GetAsync(string keyword = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = new List<WeatherForecast>()
            {
                new WeatherForecast() {Name = "namnguyen"},
                new WeatherForecast() {Name = "namnguyen1"},
                new WeatherForecast() {Name = "namnguyen2"},
                new WeatherForecast() {Name = "namnguyen3"},
                new WeatherForecast() {Name = "namnguyen4"}
            };

            return Ok(result);

        }


        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _responeCacheService.RemoveCacheResponeAsync("/weatherforecast/getall");
            return Ok();
        }

    }
}
