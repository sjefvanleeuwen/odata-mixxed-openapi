using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WebApi.ODataMixxedOpenApi.Models;
using WebApi.ODataMixxedOpenApi.Repositories;

namespace WebApi.ODataMixxedOpenApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", EdmModelBuilder.Build()));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSingleton<IRepository<WeatherForecast>, WeatherForecastRepository>();
            services.AddSwaggerGen(
                options => {
                    options.OperationFilter<EnableQueryFilter>();
                    options.DocumentFilter<CustomSwaggerFilter>();
                    var swaggerOptions = new SwaggerOptions();
                    Configuration.GetSection("Swagger").Bind(swaggerOptions);

                    foreach (var currentVersion in swaggerOptions.Versions)
                    {
                        options.SwaggerDoc(currentVersion.Name, new OpenApiInfo
                        {
                            Title = swaggerOptions.Title,
                            Version = currentVersion.Name,
                            Description = swaggerOptions.Description
                        });
                    }

                    options.DocInclusionPredicate((version, desc) =>
                    {
                        if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                        {
                            return false;
                        }
                        var versions = methodInfo.DeclaringType.GetConstructors()
                            .SelectMany(constructorInfo => constructorInfo.DeclaringType.CustomAttributes
                                .Where(attributeData => attributeData.AttributeType == typeof(ApiVersionAttribute))
                                .SelectMany(attributeData => attributeData.ConstructorArguments
                                    .Select(attributeTypedArgument => attributeTypedArgument.Value)));

                        return versions.Any(v => $"{v}" == version);
                    });

                 //   options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                 // sse if adding edm filtering is possible.
                });
            services.AddApiVersioning(apiVersioningOptions =>
            {
                apiVersioningOptions.ReportApiVersions = true;
                apiVersioningOptions.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection("Swagger").Bind(swaggerOptions);
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRoute);

                app.UseSwaggerUI(option =>
                {
                    foreach (var currentVersion in swaggerOptions.Versions)
                    {
                        option.SwaggerEndpoint(currentVersion.UiEndpoint, $"{swaggerOptions.Title} {currentVersion.Name}");
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
