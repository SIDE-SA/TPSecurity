using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class AccesFonctionnaliteMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(AccesFonctionnalite accesFonctionnalite, string HashCode), AccesFonctionnaliteResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.accesFonctionnalite);
        }
    }
}
