using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication
{
    public class GetAllAccesApplicationQueryHandler : IRequestHandler<GetAllAccesApplicationQuery, ErrorOr<PagedList<AccesApplicationResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllAccesApplicationQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<AccesApplicationResult>>> Handle(GetAllAccesApplicationQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<AccesApplication> accesApplications = _uow.AccesApplication.GetAccesApplications(request.queryParameters);

            return new PagedList<AccesApplicationResult>(_mapper.Map<List<AccesApplicationResult>>(accesApplications), accesApplications.TotalCount);
        }
    }
}
