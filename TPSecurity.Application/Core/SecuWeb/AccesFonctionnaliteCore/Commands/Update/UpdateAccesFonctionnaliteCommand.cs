using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Update;

public record UpdateAccesFonctionnaliteCommand(int Id, bool EstActif, string HashCode) :  IRequest<ErrorOr<AccesFonctionnaliteResult>>;