using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Contracts.SecuWeb.RefModule;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class RefModuleMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RefModuleResult, RefModuleResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
