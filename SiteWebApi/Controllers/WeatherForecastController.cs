using Microsoft.AspNetCore.Mvc;
using ItZnak.SiteWebApi.Controllers.Base;
using ItZnak.Infrastruction.Services;
using ItZnak.PatentsDtoLibrary.Services;

namespace SiteWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : SiteWebApiController
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    public WeatherForecastController(IConfigService conf, ILogService logger, IMongoDbContextService dbMongoContext) : base(conf, logger, dbMongoContext)
    {
    }

    [Route("checkvaluetm")]
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
}
