using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Create;

public record CreateAccesApplicationCommand(bool EstActif, int IdAccesGroupe, int IdRefApplication) : IRequest<ErrorOr<AccesApplicationResult>>;
