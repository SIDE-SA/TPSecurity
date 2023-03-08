using Mapster;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class UtilisateurAccesMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RefFonctionnalite, UtilisateurFonctionnaliteResult>()
                .Map(dest => dest.Libelle, src => src.Libelle)
                .Map(dest => dest.Permission, src => src.Permission);

            config.NewConfig<AccesModule, UtilisateurModuleResult>()
                .Map(dest => dest.IdModule, src => src.Id)
                .Map(dest => dest.Fonctionnalites, src => src.ListRefFonctionnalite.Adapt<List<UtilisateurFonctionnaliteResult>>());

            config.NewConfig<AccesApplication, UtilisateurApplicationResult>()
                .Map(dest => dest.IdApplication, src => src.Id)
                .Map(dest => dest.Modules, src => src.ListAccesModule.Adapt<List<UtilisateurModuleResult>>());

            config.NewConfig<AccesGroupe, UtilisateurAccesResult>()
                .Map(dest => dest.IdSociete, src => src.IdSociete)
                .Map(dest => dest.Applications, src => src.ListAccesApplication.Adapt<List<UtilisateurApplicationResult>>());
        }
    }
}
