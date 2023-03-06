using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Create;

public record CreateAccesGroupeCommand(string Libelle, bool EstActif, bool EstGroupeSpecial, Guid IdSociete) : IRequest<ErrorOr<AccesGroupeResult>>;
