using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Contracts.SecuWeb.RefFonctionnalite;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class RefFonctionnaliteMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RefFonctionnaliteResult, RefFonctionnaliteResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
