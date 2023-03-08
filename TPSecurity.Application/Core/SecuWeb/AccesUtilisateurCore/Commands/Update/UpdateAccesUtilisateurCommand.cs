using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Update;

public record UpdateAccesUtilisateurCommand(int Id, bool EstActif, string HashCode) :  IRequest<ErrorOr<AccesUtilisateurResult>>;
