using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetByIdUtilisateur
{
    public record GetByIdUtilisateurQuery(int Id) : IRequest<ErrorOr<UtilisateurResult>>;
}
