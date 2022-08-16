using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebApi.ODataMixxedOpenApi.Models;
using WebApi.ODataMixxedOpenApi.Repositories;

namespace WebApi.ODataMixxedOpenApi.Controllers.OData
{
    public class WeatherForecastController : ODataController, IRepositoryApiController<WeatherForecast>
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepository<WeatherForecast> _weatherForecastRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository<WeatherForecast> weatherForecastRepository)
        {
            _logger = logger;
            _weatherForecastRepository = weatherForecastRepository;
        }

        [EnableQuery]
        [HttpGet("odata/WeatherForecast")]
        [HttpGet("odata/WeatherForecast/$count")]
        public ActionResult<IQueryable<WeatherForecast>> GetAll()
        {
            var x = _weatherForecastRepository.GetAll().AsQueryable();
            return Ok(x);
        }
    }
}
