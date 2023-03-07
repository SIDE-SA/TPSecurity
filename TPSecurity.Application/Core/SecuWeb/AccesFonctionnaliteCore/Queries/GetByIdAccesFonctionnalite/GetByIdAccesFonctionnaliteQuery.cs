using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetByIdAccesFonctionnalite
{
    public record GetByIdAccesFonctionnaliteQuery(int Id) : IRequest<ErrorOr<AccesFonctionnaliteResult>>;
}
