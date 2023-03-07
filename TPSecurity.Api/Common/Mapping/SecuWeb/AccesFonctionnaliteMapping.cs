using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesFonctionnalite;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class AccesFonctionnaliteMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AccesFonctionnaliteResult, AccesFonctionnaliteResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
