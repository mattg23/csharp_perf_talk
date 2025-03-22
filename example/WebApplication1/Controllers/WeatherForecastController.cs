using Microsoft.AspNetCore.Mvc;
using Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherDataProvider weatherDataProvider;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherDataProvider weatherDataProvider)
    {
        _logger = logger;
        this.weatherDataProvider = weatherDataProvider;
    }

    [HttpGet]
    public IEnumerable<WeatherDataSummary> Get()
    {
        return weatherDataProvider.GetSummary();
    }
}
