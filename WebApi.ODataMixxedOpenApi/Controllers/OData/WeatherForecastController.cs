using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebApi.ODataMixxedOpenApi.Models;
using WebApi.ODataMixxedOpenApi.Repositories;

namespace WebApi.ODataMixxedOpenApi.Controllers.OData
{
    [ApiVersion("1.0-odata")]
    [ApiVersion("1.0")]
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
            // Let's say the user is only allowed to qyery temperatures in amsterdam.
            var x = _weatherForecastRepository.GetAll().Where(p=>p.City == "Amsterdam").AsQueryable().Where(p=>p.City.Any()).AsQueryable();
            return Ok(x);
        }
    }
}
