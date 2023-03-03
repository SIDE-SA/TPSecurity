using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation.AspNetCore;
using FluentValidation;
using TPSecurity.Application.Common.Interfaces;
using TPSecurity.Application.Common.Mapping;

namespace TPSecurity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddFluentValidation();
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddMappings();
        return services;
    }
}
