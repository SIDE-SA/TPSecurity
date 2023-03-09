using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetAllRefModule;

public record GetAllRefModuleQuery(RefModuleParameters queryParameters) : IRequest<ErrorOr<PagedList<RefModuleResult>>>;