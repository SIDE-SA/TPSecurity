using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetAllAccesFonctionnalite
{
    public class GetAllAccesFonctionnaliteQueryHandler : IRequestHandler<GetAllAccesFonctionnaliteQuery, ErrorOr<PagedList<AccesFonctionnaliteResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllAccesFonctionnaliteQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<AccesFonctionnaliteResult>>> Handle(GetAllAccesFonctionnaliteQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<AccesFonctionnalite> accesFonctionnalite = _uow.AccesFonctionnalite.GetAccesFonctionnalites(request.queryParameters);

            return new PagedList<AccesFonctionnaliteResult>(_mapper.Map<List<AccesFonctionnaliteResult>>(accesFonctionnalite), accesFonctionnalite.TotalCount);
        }
    }
}
