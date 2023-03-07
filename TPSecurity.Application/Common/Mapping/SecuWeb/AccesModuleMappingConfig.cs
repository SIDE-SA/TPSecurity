using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class AccesModuleMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(AccesModule accesModule, string HashCode), AccesModuleResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.accesModule);
        }
    }
}
