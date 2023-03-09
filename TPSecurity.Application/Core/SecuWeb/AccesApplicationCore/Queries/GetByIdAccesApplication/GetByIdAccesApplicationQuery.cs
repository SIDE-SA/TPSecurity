using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetByIdAccesApplication;

public record GetByIdAccesApplicationQuery(int Id) : IRequest<ErrorOr<AccesApplicationResult>>;