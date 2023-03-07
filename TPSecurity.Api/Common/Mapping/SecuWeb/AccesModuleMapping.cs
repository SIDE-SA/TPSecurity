using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesModule;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class AccesModuleMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AccesModuleResult, AccesModuleResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
