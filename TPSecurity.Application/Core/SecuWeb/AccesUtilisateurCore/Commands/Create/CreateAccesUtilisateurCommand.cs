using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Create;

public record CreateAccesUtilisateurCommand(bool EstActif, int IdAccesGroupe, int IdUtilisateur) : IRequest<ErrorOr<AccesUtilisateurResult>>;