using Microsoft.EntityFrameworkCore;
using PM.Application.Models.Country;
using PM.Application.Persistence.Repositories;
using PM.Domain.CountryAggregate;
using PM.Domain.CountryAggregate.ValueObjects;

namespace PM.Infrastructure.Persistence.Repositories;

public class CountryReadOnlyRepository : ICountryReadOnlyRepository
{
    private readonly PMDbContext _dbContext;

    public CountryReadOnlyRepository(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Country?> GetById(CountryId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Countries.Include(c => c.Provinces)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<List<CountryDto>> GetList(CancellationToken cancellationToken = default)
    {
        return _dbContext.Countries.AsNoTracking()
            .Select(c => new CountryDto
            {
                Id = c.Id.Value,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ProvinceExists(ProvinceId provinceId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Countries.AsNoTracking()
            .AnyAsync(c => c.Provinces.Any(p => p.Id == provinceId), cancellationToken);
    }
}

