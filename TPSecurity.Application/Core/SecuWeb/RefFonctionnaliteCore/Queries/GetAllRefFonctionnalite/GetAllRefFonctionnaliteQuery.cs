using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetAllRefFonctionnalite;

public record GetAllRefFonctionnaliteQuery(RefFonctionnaliteParameters queryParameters) : IRequest<ErrorOr<PagedList<RefFonctionnaliteResult>>>;