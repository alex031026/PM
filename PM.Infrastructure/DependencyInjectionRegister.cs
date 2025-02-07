using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PM.Application.Common.Providers;
using PM.Application.Persistence.Repositories;
using PM.Infrastructure.Persistence;
using PM.Infrastructure.Persistence.Interceptors;
using PM.Infrastructure.Persistence.Repositories;
using PM.Infrastructure.Providers;

namespace PM.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSqlLitePersistence(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IHashProvider, HashProvider>();

        return services;
    }

    public static IServiceCollection AddSqlLitePersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PMDbContext>(options =>
            options.UseSqlite("Data Source=localdb.db"));

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICountryReadOnlyRepository, CountryReadOnlyRepository>();

        return services;
    }
}
