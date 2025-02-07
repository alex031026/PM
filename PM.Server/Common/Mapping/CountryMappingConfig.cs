using Mapster;
using PM.Contracts.Country;

namespace PM.WebApi.Common.Mapping;

public class CountryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PM.Domain.CountryAggregate.Country, CountryViewItem>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<PM.Domain.CountryAggregate.Entities.Province, CountryProvinceViewItem>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}