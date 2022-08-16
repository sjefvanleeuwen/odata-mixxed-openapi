using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace WebApi.ODataMixxedOpenApi
{
    public class CustomSwaggerFilter : IDocumentFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CustomSwaggerFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var audience = httpContextAccessor.HttpContext.Request.Query["audience"];
            if (string.IsNullOrEmpty(audience))
                return;
            var nonMobileRoutes = swaggerDoc.Paths
                .Where(x => !x.Key.ToLower().Contains(audience))
                .ToList();
            nonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
        }
    }
}