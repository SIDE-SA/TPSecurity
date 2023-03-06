using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Delete
{
    public record DeleteAccesGroupeCommand(int id) : IRequest<ErrorOr<Deleted>>;
}
