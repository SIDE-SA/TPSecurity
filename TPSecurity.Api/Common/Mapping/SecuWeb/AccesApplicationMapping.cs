using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesApplication;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class AccesApplicationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AccesApplicationResult, AccesApplicationResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
