using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Create;

public record CreateAccesFonctionnaliteCommand(bool EstActif, int IdAccesModule, int IdRefFonctionnalite) : IRequest<ErrorOr<AccesFonctionnaliteResult>>;
