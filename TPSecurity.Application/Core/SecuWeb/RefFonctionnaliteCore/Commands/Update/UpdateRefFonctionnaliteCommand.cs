using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Update;

public record UpdateRefFonctionnaliteCommand(int Id, string Libelle, bool EstActif, bool EstDefaut, string HashCode) :  IRequest<ErrorOr<RefFonctionnaliteResult>>;
