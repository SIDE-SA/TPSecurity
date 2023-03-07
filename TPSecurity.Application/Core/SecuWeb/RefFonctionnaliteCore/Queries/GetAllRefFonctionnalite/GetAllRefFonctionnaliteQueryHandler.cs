using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetAllRefFonctionnalite
{
    public class GetAllRefFonctionnaliteQueryHandler : IRequestHandler<GetAllRefFonctionnaliteQuery, ErrorOr<PagedList<RefFonctionnaliteResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllRefFonctionnaliteQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<RefFonctionnaliteResult>>> Handle(GetAllRefFonctionnaliteQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<RefFonctionnalite> refFonctionnalites = _uow.RefFonctionnalite.GetRefFonctionnalites(request.queryParameters);

            return new PagedList<RefFonctionnaliteResult>(_mapper.Map<List<RefFonctionnaliteResult>>(refFonctionnalites), refFonctionnalites.TotalCount);
        }
    }
}
