using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetByIdRefModule;

public record GetByIdRefModuleQuery(int Id) : IRequest<ErrorOr<RefModuleResult>>;