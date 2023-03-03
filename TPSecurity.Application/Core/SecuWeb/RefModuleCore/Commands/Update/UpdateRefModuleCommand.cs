using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Update;

public record UpdateRefModuleCommand(int Id, string Libelle, bool EstActif, int IdRefApplication, string HashCode) :  IRequest<ErrorOr<RefModuleResult>>;
