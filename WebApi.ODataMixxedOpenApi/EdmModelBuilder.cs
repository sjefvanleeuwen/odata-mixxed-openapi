using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using WebApi.ODataMixxedOpenApi.Models;

namespace WebApi.ODataMixxedOpenApi
{
    public static class EdmModelBuilder
    {
        public static IEdmModel Build()
        {
            // create OData builder instance
            var builder = new ODataConventionModelBuilder();
            var books = builder.EntitySet<WeatherForecast>("WeatherForecast").EntityType.HasKey(x => x.Id);
            books.Count().Filter().OrderBy().Expand().Select().Page(int.MaxValue, 25);
            return builder.GetEdmModel();
        }
    }
}
