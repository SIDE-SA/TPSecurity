using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Create;

public record CreateRefFonctionnaliteCommand(string Libelle, bool EstActif, bool EstDefaut, string Permission, int IdRefModule) : IRequest<ErrorOr<RefFonctionnaliteResult>>;
