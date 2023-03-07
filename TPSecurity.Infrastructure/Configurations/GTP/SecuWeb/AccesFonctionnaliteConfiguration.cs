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
    public class AccesFonctionnaliteConfiguration : IEntityTypeConfiguration<AccesFonctionnaliteDTO>
    {
        public void Configure(EntityTypeBuilder<AccesFonctionnaliteDTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_AccesFonctionnalite");

            builder.ToTable("AccesFonctionnalite", "SecuWeb");

            builder.Property(e => e.Id);
            builder.Property(e => e.DateCreation).HasColumnType("datetime");
            builder.Property(e => e.DateModification).HasColumnType("datetime");
            builder.Property(e => e.UserCreation).HasMaxLength(40);
            builder.Property(e => e.UserModification).HasMaxLength(40);

            builder.HasOne(d => d.IdAccesModuleNavigation).WithMany(p => p.AccesFonctionnalites)
                .HasForeignKey(d => d.IdAccesModule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_RefFonctionnalite_AccesModule");

            builder.HasOne(d => d.IdRefFonctionnaliteNavigation).WithMany(p => p.AccesFonctionnalites)
                .HasForeignKey(d => d.IdRefFonctionnalite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesFonctionnalite_RefFonctionnalite");
        }
    }
}
