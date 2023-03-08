using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesUtilisateur;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class AccesUtilisateurMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AccesUtilisateurResult, AccesUtilisateurResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
