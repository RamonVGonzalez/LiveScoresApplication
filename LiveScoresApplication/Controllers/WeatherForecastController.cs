using Microsoft.AspNetCore.Mvc;

namespace LiveScoresApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "GetFromOtherApi")]
        public async Task<IActionResult> GetScoresFromLivescores()
        {
            var result = await new LiveScore().GetDataFromLivescores();

            return Ok(result);
        }

        [HttpGet(Name = "GetLiveScores")]
        public async Task<IActionResult> GetScoresFromFootballDataDotOrg()
        {
            var result = await new LiveScore().GetDataFromFootballDataDotOrg();

            return Ok(result);
        }
    }
}