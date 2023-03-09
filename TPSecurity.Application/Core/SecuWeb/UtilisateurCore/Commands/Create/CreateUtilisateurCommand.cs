using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Create;

public record CreateUtilisateurCommand(string Nom, string Prenom, string Email, bool EstActif) : IRequest<ErrorOr<UtilisateurResult>>;