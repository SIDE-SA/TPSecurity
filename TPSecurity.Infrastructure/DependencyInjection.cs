using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Services;
using TPSecurity.Application.Common.Interfaces.Services.GeneralConcept;
using TPSecurity.Infrastructure.Options;
using TPSecurity.Infrastructure.Persistence;
using TPSecurity.Infrastructure.Persistence.Interceptors;
using TPSecurity.Infrastructure.Services;
using TPSecurity.Infrastructure.Services.APIs.GeneralConcept;

namespace TPSecurity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GeneralConceptOptions>(configuration.GetSection("GeneralConcept"));
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.ConfigureOptions<DatabaseOptionsSetup>();
            services.AddDbContext<ApplicationContextGTP>((serviceProvider, dbContextOptionBuilder) =>
            {
                var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

                dbContextOptionBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
                dbContextOptionBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });

            services.AddScoped<IUnitOfWorkGTP, UnitOfWorkGTP>();
            services.AddScoped<IGeneralConceptService, GeneralConceptService>();
            return services;
        }
    }
}
