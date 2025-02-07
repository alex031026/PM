using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PM.Application.Common.Behaviors;
using System.Reflection;

namespace PM.Application
{
    public static class DependencyInjectionRegister
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //services.AddScoped(
            //        typeof(IPipelineBehavior<,>),
            //        typeof(ValidationBehavior<,>));


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
            return services;
        }
    }
}
