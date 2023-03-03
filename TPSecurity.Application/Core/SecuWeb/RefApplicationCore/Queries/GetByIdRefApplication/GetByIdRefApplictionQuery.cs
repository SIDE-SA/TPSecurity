using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetByIdRefApplication
{
    public record GetByIdRefApplicationQuery(int Id) : IRequest<ErrorOr<RefApplicationResult>>;
}
