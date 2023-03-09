using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication;

public record GetAllAccesApplicationQuery(AccesApplicationParameters queryParameters) : IRequest<ErrorOr<PagedList<AccesApplicationResult>>>;