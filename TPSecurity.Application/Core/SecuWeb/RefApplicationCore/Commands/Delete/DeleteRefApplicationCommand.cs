using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Delete
{
    public record DeleteRefApplicationCommand(int id) : IRequest<ErrorOr<Deleted>>;
}
