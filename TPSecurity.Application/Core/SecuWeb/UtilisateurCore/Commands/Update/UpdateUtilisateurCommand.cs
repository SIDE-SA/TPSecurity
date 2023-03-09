using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Update;

public record UpdateUtilisateurCommand(int Id, string Nom, string Prenom, string Email, bool EstActif, string HashCode) :  IRequest<ErrorOr<UtilisateurResult>>;
