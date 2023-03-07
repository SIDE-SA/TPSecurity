
using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Contracts.SecuWeb.AccesGroupe;

namespace TPSecurity.Api.Common.Mapping.SecuWeb
{
    public class AccesGroupeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AccesGroupeResult, AccesGroupeResponse>()
                .Map(dest => dest.Identifiant, src => src.Id)
                .Map(dest => dest, src => src);
        }
    }
}
