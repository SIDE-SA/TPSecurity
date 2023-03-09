using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetAllAccesGroupe;

public record GetAllAccesGroupeQuery(AccesGroupeParameters queryParameters) : IRequest<ErrorOr<PagedList<AccesGroupeResult>>>;