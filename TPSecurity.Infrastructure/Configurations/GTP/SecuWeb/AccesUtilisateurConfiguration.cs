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
    public class AccesUtilisateurConfiguration : IEntityTypeConfiguration<AccesUtilisateurDTO>
    {
        public void Configure(EntityTypeBuilder<AccesUtilisateurDTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_AccesUtilisateur");

            builder.ToTable("AccesUtilisateur", "SecuWeb");

            builder.Property(e => e.Id);
            builder.Property(e => e.DateCreation).HasColumnType("datetime");
            builder.Property(e => e.DateModification).HasColumnType("datetime");
            builder.Property(e => e.UserCreation).HasMaxLength(40);
            builder.Property(e => e.UserModification).HasMaxLength(40);

            builder.HasOne(d => d.IdAccesGroupeNavigation).WithMany(p => p.AccesUtilisateurs)
                .HasForeignKey(d => d.IdAccesGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesUtilisateur_AccesGroupe");

            builder.HasOne(d => d.IdUtilisateurNavigation).WithMany(p => p.AccesUtilisateurs)
                .HasForeignKey(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_AccesUtilisateur_Utilisateur");
        }
    }
}
