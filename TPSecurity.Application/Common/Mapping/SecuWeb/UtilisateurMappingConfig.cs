using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class UtilisateurMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(Utilisateur utilisateur, string HashCode), UtilisateurResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.utilisateur);
        }
    }
}
