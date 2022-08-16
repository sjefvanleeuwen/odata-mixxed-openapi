using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.ODataMixxedOpenApi.Models;

namespace WebApi.ODataMixxedOpenApi.Repositories
{
    public class WeatherForecastRepository : IRepository<WeatherForecast>
    {
        private List<WeatherForecast> forecasts = new List<WeatherForecast>();

        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] Cities = new[]
{
            "Amsterdam", "Rotterdam", "The Hague", "Utrecht"
        };

        public WeatherForecastRepository()
        {
            // create some fakedata
            var rng = new Random();
            forecasts = Enumerable.Range(1, 1000).Select(index => new WeatherForecast
            {

                Id = Guid.NewGuid().ToString(),
                City = Cities[rng.Next(Cities.Length)],
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();
        }

        public IEnumerable<WeatherForecast> GetAll()
        {
            return forecasts.AsEnumerable();
        }
    }
}
