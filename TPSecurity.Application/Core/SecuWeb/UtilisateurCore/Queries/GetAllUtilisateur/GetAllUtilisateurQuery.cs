using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetAllUtilisateur
{
    public record GetAllUtilisateurQuery(UtilisateurParameters queryParameters) : IRequest<ErrorOr<PagedList<UtilisateurResult>>>;
}
