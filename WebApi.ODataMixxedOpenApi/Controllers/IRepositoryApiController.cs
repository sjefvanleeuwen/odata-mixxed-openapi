using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApi.ODataMixxedOpenApi.Controllers
{


    public interface IRepositoryApiController<T>
    {
        ActionResult<IQueryable<T>> GetAll();
    }
}
