using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TPSecurity.Infrastructure.Configurations.GTP;
using TPSecurity.Infrastructure.Models.SecuWeb;
using TPSecurity.Infrastructure.Persistence.Interceptors;
using TPUtilities.Converter;

namespace TPSecurity.Infrastructure.Persistence
{
    public partial class ApplicationContextGTP : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitytSaveChangesInterceptor;
        public ApplicationContextGTP(
            DbContextOptions<ApplicationContextGTP> options,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            AuditableEntitySaveChangesInterceptor auditableEntitytSaveChangesInterceptor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _auditableEntitytSaveChangesInterceptor = auditableEntitytSaveChangesInterceptor;
        }      

        public virtual DbSet<RefApplicationDTO> RefApplication { get; set; }
        public virtual DbSet<RefModuleDTO> RefModule { get; set; }
        public virtual DbSet<AccesGroupeDTO> AccesGroupe { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connectionString = _configuration.GetConnectionString("GTP");
                if (string.IsNullOrWhiteSpace(connectionString)) throw new IOException();
                optionsBuilder.UseSqlServer(connectionString);
            }

            optionsBuilder.AddInterceptors(_auditableEntitytSaveChangesInterceptor);

            optionsBuilder.ConfigureWarnings(wc => wc.Ignore(RelationalEventId.BoolWithDefaultWarning));
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateTimeToLocalKind.DateTimeConverterToLocalKind(modelBuilder);


            //Le filtre permet de ne pas charger les configurations de toute l'assembly mais de ciblé celle lié au context (entité ou GTP)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
                                                        t => t.GetInterfaces()
                                                               .Any(i => i.IsGenericType
                                                                        && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                                                                        && t.Namespace.StartsWith(typeof(ConfigurationGTP).Namespace))
                                                        );
            base.OnModelCreating(modelBuilder);
        }        
    }
}
