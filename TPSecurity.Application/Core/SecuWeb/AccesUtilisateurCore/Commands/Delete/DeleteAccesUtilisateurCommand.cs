using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Delete
{
    public record DeleteAccesUtilisateurCommand(int id) : IRequest<ErrorOr<Deleted>>;
}
