using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Delete;

public record DeleteAccesModuleCommand(int id) : IRequest<ErrorOr<Deleted>>;