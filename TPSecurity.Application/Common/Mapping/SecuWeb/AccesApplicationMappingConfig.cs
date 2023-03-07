using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class AccesApplicationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(AccesApplication accesApplication, string HashCode), AccesApplicationResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.accesApplication);
        }
    }
}
