﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Clients
{
    public interface IWeatherClient
    {
        Task<WeatherForecast[]> GetWeatherAsync();
    }

    public class WeatherClient : IWeatherClient
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly HttpClient client;

        public WeatherClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<WeatherForecast[]> GetWeatherAsync()
        {
            var responseMessage = await this.client.GetAsync("/weatherforecast");
            var stream = await responseMessage.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<WeatherForecast[]>(stream, options);
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
