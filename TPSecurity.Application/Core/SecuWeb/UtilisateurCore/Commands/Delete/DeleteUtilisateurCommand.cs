using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Delete
{
    public record DeleteUtilisateurCommand(int id) : IRequest<ErrorOr<Deleted>>;
}
