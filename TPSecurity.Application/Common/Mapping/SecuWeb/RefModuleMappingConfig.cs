using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class RefModuleMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(RefModule refModule, string HashCode), RefModuleResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.refModule);

            config.NewConfig<RefModule, RefModuleResult>()
                .Map(dest => dest, src => src);
        }
    }
}
