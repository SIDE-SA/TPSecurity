using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetByIdAccesGroupe
{
    public record GetByIdAccesGroupeQuery(int Id) : IRequest<ErrorOr<AccesGroupeResult>>;
}
