using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class RefFonctionnaliteMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(RefFonctionnalite refFonctionnalite, string HashCode), RefFonctionnaliteResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.refFonctionnalite);
        }
    }
}
