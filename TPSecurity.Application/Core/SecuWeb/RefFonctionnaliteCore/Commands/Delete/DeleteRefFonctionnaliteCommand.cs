using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Delete
{
    public record DeleteRefFonctionnaliteCommand(int id) : IRequest<ErrorOr<Deleted>>;
}
