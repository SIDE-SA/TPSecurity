using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Update;

public record UpdateRefApplicationCommand(int Id, string Libelle, bool EstActif, string HashCode) :  IRequest<ErrorOr<RefApplicationResult>>;
