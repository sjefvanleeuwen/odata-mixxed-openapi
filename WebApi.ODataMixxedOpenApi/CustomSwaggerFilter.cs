using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
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
            //var audience = httpContextAccessor.HttpContext.Request.Query["audience"];
            foreach (var (item, operation, response, content) in from item in swaggerDoc.Paths
                                                                 from operation in item.Value.Operations
                                                                 from response in operation.Value.Responses
                                                                 from content in response.Value.Content
                                                                 where content.Key.ToLower().Contains("application/json;odata")
                                                                 select (item, operation, response, content))
            {
                swaggerDoc.Paths[item.Key].Operations[operation.Key].Responses[response.Key].Content.Remove(content.Key);
            }


            //var nonMobileRoutes = swaggerDoc.Paths
            //    .Where(x => !x.Key.ToLower().Contains(audience))
            //    .ToList();
            //nonMobileRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
        }
    }

    public class SwaggerOptions
    {
        public string Title { get; set; }
        public string JsonRoute { get; set; }
        public string Description { get; set; }
        public List<Version> Versions { get; set; }

        public class Version
        {
            public string Name { get; set; }
            public string UiEndpoint { get; set; }
        }
    }
}