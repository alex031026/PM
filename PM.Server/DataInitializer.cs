using Microsoft.EntityFrameworkCore;
using PM.Domain.CountryAggregate;
using PM.Domain.CountryAggregate.ValueObjects;
using PM.Infrastructure.Persistence;

namespace PM.WebApi;

public static class DataInitializer
{
    public static void UseDatabaseInitialization(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<PMDbContext>();
        var logger = services.GetRequiredService<ILogger<PMDbContext>>();

        try
        {
            logger.LogInformation("Applying migrations...");
            dbContext.Database.Migrate(); // Apply migrations automatically

            logger.LogInformation("Seeding database...");
            SeedData(dbContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }

    private static void SeedData(PMDbContext dbContext)
    {
        if (dbContext.Countries.Any()) return; 
        
        //{ name: 'USA', provinces: ['California', 'Texas', 'New York'] },
        //{ name: 'Canada', provinces: ['Ontario', 'Quebec', 'British Columbia'] },
        //{ name: 'India', provinces: ['Maharashtra', 'Karnataka', 'Delhi'] }

        {

            var country = Country.Create("USA");
            country.AddProvince("California");
            country.AddProvince("Texas");
            country.AddProvince("New York");
            dbContext.Add(country);
        }

        {
            var country = Country.Create("Canada");
            country.AddProvince("Ontario");
            country.AddProvince("Quebec");
            country.AddProvince("British Columbia");
            dbContext.Add(country);
        }

        {
            var country = Country.Create("India");
            country.AddProvince("Maharashtra");
            country.AddProvince("Karnataka");
            country.AddProvince("Delhi");
            dbContext.Add(country);
        }

        dbContext.SaveChanges();
    }
}

