using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.ODataMixxedOpenApi.Models;
using WebApi.ODataMixxedOpenApi.Repositories;

namespace WebApi.ODataMixxedOpenApi.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase, IRepositoryApiController<WeatherForecast>
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepository<WeatherForecast> _weatherForecastRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepository<WeatherForecast> weatherForecastRepository)
        {
            _logger = logger;
            _weatherForecastRepository = weatherForecastRepository;
        }

        [HttpGet("api/WeatherForecast")]
        public ActionResult<IQueryable<WeatherForecast>> GetAll()
        {
            return Ok(_weatherForecastRepository.GetAll().AsQueryable());
        }
    }
}