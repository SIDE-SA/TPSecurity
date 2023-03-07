using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetAllAccesModule
{
    public record GetAllAccesModuleQuery(AccesModuleParameters queryParameters) : IRequest<ErrorOr<PagedList<AccesModuleResult>>>;
}
