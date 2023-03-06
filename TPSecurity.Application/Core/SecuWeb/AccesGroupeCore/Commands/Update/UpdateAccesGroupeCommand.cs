using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Update;

public record UpdateAccesGroupeCommand(int Id, string Libelle, bool EstActif, bool EstGroupeSpecial, string HashCode) :  IRequest<ErrorOr<AccesGroupeResult>>;
