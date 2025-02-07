using Mapster;
using PM.Application.User.Commands;
using PM.Contracts.Auth;

namespace PM.WebApi.Common.Mapping;

public class AuthMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PM.Domain.UserAggregate.User, RegisterResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProvinceId, src => src.ProvinceId.Value);
    }
}