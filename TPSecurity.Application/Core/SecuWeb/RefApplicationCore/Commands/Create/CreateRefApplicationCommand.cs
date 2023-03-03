using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Create;

public record CreateRefApplicationCommand(string Libelle, bool EstActif) : IRequest<ErrorOr<RefApplicationResult>>;
