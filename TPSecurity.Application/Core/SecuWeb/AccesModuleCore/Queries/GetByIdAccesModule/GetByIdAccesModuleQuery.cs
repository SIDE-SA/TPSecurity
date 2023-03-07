using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetByIdAccesModule
{
    public record GetByIdAccesModuleQuery(int Id) : IRequest<ErrorOr<AccesModuleResult>>;
}
