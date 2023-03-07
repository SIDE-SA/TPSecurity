using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetAllAccesFonctionnalite
{
    public record GetAllAccesFonctionnaliteQuery(AccesFonctionnaliteParameters queryParameters) : IRequest<ErrorOr<PagedList<AccesFonctionnaliteResult>>>;
}
