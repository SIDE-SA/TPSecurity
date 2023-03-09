using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Update;

public record UpdateAccesApplicationCommand(int Id, bool EstActif, string HashCode) :  IRequest<ErrorOr<AccesApplicationResult>>;