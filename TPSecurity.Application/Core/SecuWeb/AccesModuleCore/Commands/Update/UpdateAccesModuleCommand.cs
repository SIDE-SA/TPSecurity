using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Update;

public record UpdateAccesModuleCommand(int Id, bool EstActif, string HashCode) :  IRequest<ErrorOr<AccesModuleResult>>;
