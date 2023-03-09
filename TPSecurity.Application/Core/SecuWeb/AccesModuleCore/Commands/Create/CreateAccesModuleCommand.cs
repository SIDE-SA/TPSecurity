using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Create;

public record CreateAccesModuleCommand(bool EstActif, int IdAccesApplication, int IdRefModule) : IRequest<ErrorOr<AccesModuleResult>>;