using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using TPSecurity.Infrastructure.Models.SecuWeb;

namespace TPSecurity.Infrastructure.Configurations.GTP.SecuWeb
{
    public class AccesApplicationConfiguration : IEntityTypeConfiguration<AccesApplicationDTO>
    {
        public void Configure(EntityTypeBuilder<AccesApplicationDTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_AccesApplication");

            builder.ToTable("AccesApplication", "SecuWeb");

            builder.Property(e => e.Id);
            builder.Property(e => e.DateCreation).HasColumnType("datetime");
            builder.Property(e => e.DateModification).HasColumnType("datetime");
            builder.Property(e => e.EstActif).HasMaxLength(50);
            builder.Property(e => e.UserCreation).HasMaxLength(40);
            builder.Property(e => e.UserModification).HasMaxLength(40);

            builder.HasOne(d => d.IdAccesGroupeNavigation).WithMany(p => p.AccesApplications)
                .HasForeignKey(d => d.IdAccesGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesApplication_AccesGroupe");

            builder.HasOne(d => d.IdRefApplicationNavigation).WithMany(p => p.AccesApplications)
                .HasForeignKey(d => d.IdRefApplication)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesApplication_RefApplication");
        }
    }
}
