using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetAllRefApplication;

public record GetAllRefApplicationQuery(RefApplicationParameters queryParameters) : IRequest<ErrorOr<PagedList<RefApplicationResult>>>;