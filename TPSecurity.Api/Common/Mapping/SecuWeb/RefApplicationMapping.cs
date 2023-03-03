
using Mapster;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Contracts.SecuWeb.RefApplication;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class RefApplicationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RefApplicationResult, RefApplicationResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
