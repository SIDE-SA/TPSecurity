using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetByIdRefFonctionnalite;

public record GetByIdRefFonctionnaliteQuery(int Id) : IRequest<ErrorOr<RefFonctionnaliteResult>>;