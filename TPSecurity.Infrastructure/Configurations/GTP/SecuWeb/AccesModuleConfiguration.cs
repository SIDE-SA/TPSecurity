using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPSecurity.Infrastructure.Models.SecuWeb;

namespace TPSecurity.Infrastructure.Configurations.GTP.SecuWeb
{
    public class AccesModuleConfiguration : IEntityTypeConfiguration<AccesModuleDTO>
    {
        public void Configure(EntityTypeBuilder<AccesModuleDTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_AccesModule");

            builder.ToTable("AccesModule", "SecuWeb");

            builder.Property(e => e.Id);
            builder.Property(e => e.DateCreation).HasColumnType("datetime");
            builder.Property(e => e.DateModification).HasColumnType("datetime");
            builder.Property(e => e.EstActif).HasMaxLength(50);
            builder.Property(e => e.UserCreation).HasMaxLength(40);
            builder.Property(e => e.UserModification).HasMaxLength(40);

            builder.HasOne(d => d.IdAccesApplicationNavigation).WithMany(p => p.AccesModules)
                .HasForeignKey(d => d.IdAccesApplication)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesModule_AccesApplication");

            builder.HasOne(d => d.IdRefModuleNavigation).WithMany(p => p.AccesModules)
                .HasForeignKey(d => d.IdRefModule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesModule_RefApplication");
        }
    }
}
