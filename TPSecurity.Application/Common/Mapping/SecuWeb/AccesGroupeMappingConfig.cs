using Mapster;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Mapping.SecuWeb
{
    public class AccesGroupeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(AccesGroupe accesGroupe, string HashCode), AccesGroupeResult>()
                .Map(dest => dest.HashCode, src => src.HashCode)
                .Map(dest => dest, src => src.accesGroupe);
        }
    }
}
