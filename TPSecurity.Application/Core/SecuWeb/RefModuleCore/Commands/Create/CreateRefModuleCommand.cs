using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Create;

public record CreateRefModuleCommand(string Libelle, bool EstActif, int IdRefApplication) : IRequest<ErrorOr<RefModuleResult>>;