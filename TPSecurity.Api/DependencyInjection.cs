using Microsoft.AspNetCore.Mvc.Infrastructure;
using TPSecurity.Api.Common.Mapping;
using TPSecurity.Api.Errors;
using FluentValidation;
using TPSecurity.Api.Common.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using TPSecurity.Api.Behaviour;
using TPSecurity.Application.Common.Interfaces.Services;
using TPSecurity.Api.Services;
using TPSecurity.API.Services;
using TPSecurity.Infrastructure.Interfaces;

namespace TPSecurity.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });
            services.AddControllers();            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HeaderBehaviour<,>));
            services.AddSingleton<ProblemDetailsFactory, TPProblemDetailsFactory>();
            services.AddMappings();
            services.AddFluentValidation();
            services.AddValidatorsFromAssemblyContaining<AssemblyReference>();
            services.AddScoped<IUserRequestService, UserRequestService>();
            services.AddScoped<ITenantService, TenantService>();
            return services;
        }
    }
}
