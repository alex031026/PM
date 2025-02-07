using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PM.Application.Persistence;
using PM.Domain.Common.Models;
using PM.Domain.CountryAggregate;
using PM.Domain.UserAggregate;
using PM.Infrastructure.Persistence.Interceptors;

namespace PM.Infrastructure.Persistence;

public class PMDbContext : DbContext, IUnitOfWork
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public PMDbContext(
        DbContextOptions<PMDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor
    ) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Country> Countries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(PMDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(_publishDomainEventsInterceptor);

        base.OnConfiguring(optionsBuilder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}

