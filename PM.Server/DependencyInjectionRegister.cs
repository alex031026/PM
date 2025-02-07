using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

namespace PM.WebApi;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PM API", Version = "v1" });
        });

        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, Common.Errors.ProblemDetailsFactory>();
        //services.AddMappings();
        return services;
    }
}