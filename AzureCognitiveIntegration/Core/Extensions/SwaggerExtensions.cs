using System.Reflection;
using AzureCognitiveIntegration.Config;
using Microsoft.OpenApi.Models;

namespace AzureCognitiveIntegration.Core.Extensions;

public static class SwaggerExtensions
{
    public static void CustomAddSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerConf = configuration.GetSwaggerSettings();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Url = new Uri("https://cognitiveconnector.com/"),
                },
                Title = swaggerConf.Title,
                Version = swaggerConf.Version,
                Description = swaggerConf.Description
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
    
    public static void CustomUseSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        var swaggerConf = configuration.GetSwaggerSettings();
        if (!swaggerConf.Enabled) return;
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", swaggerConf.Title);
            c.InjectStylesheet("/swagger-ui/custom.css");
            c.DocumentTitle = swaggerConf.Title;
            c.RoutePrefix = "swagger";
        });
    }
}