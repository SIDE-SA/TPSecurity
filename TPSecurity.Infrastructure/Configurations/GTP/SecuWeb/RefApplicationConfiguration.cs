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
    public class RefApplicationConfiguration : IEntityTypeConfiguration<RefApplicationDTO>
    {
        public void Configure(EntityTypeBuilder<RefApplicationDTO> builder)
        {            
            builder.HasKey(e => e.Id).HasName("PK_RefApplication");

            builder.ToTable("RefApplication", "SecuWeb");

            builder.Property(e => e.Id);
            builder.Property(e => e.DateCreation).HasColumnType("datetime");
            builder.Property(e => e.DateModification).HasColumnType("datetime");
            builder.Property(e => e.Libelle).HasMaxLength(50);
            builder.Property(e => e.UserCreation).HasMaxLength(40);
            builder.Property(e => e.UserModification).HasMaxLength(40);
        }
    }
}
