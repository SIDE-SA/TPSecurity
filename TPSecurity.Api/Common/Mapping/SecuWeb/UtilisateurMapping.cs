using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Contracts.SecuWeb.Utilisateur;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class UtilisateurMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UtilisateurResult, UtilisateurResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
