using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Delete
{
    public record DeleteRefModuleCommand(int id) : IRequest<ErrorOr<Deleted>>;
}
