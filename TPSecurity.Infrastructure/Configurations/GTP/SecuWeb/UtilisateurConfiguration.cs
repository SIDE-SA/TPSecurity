using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TPSecurity.Infrastructure.Models.SecuWeb;

namespace TPSecurity.Infrastructure.Configurations.GTP.SecuWeb
{
    public class UtilisateurConfiguration : IEntityTypeConfiguration<UtilisateurDTO>
    {
        public void Configure(EntityTypeBuilder<UtilisateurDTO> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Utilisateur");

            builder.ToTable("Utilisateur", "SecuWeb");

            builder.Property(e => e.Id);
            builder.Property(e => e.DateCreation).HasColumnType("datetime");
            builder.Property(e => e.DateModification).HasColumnType("datetime");
            builder.Property(e => e.Email).HasMaxLength(200);
            builder.Property(e => e.Nom).HasMaxLength(100);
            builder.Property(e => e.Prenom).HasMaxLength(100);
            builder.Property(e => e.UserCreation).HasMaxLength(40);
            builder.Property(e => e.UserModification).HasMaxLength(40);
        }
    }
}
