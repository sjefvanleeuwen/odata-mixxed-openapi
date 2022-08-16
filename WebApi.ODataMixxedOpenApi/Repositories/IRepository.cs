using System.Collections.Generic;

namespace WebApi.ODataMixxedOpenApi.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
    }
}
