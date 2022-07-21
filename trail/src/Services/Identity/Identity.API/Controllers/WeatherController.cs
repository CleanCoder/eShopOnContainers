using ID.eShop.Services.Identity.API.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherClient _client;

        public WeatherController(IWeatherClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            var forecasts =  _client.GetWeatherAsync().GetAwaiter().GetResult();

            //var forecasts = new WeatherForecast[] { new WeatherForecast() { Date = System.DateTime.Now, TemperatureC = 65 } };

            return View(new WeatherForecastModel { Forecasts = forecasts });
        }
    }

    public class WeatherForecastModel
    {
        public WeatherForecast[] Forecasts { get; set; }
    }
}
