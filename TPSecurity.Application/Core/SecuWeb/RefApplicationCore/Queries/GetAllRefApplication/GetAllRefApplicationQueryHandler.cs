using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetAllRefApplication
{
    public class GetAllRefApplicationQueryHandler : IRequestHandler<GetAllRefApplicationQuery, ErrorOr<PagedList<RefApplicationResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllRefApplicationQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<RefApplicationResult>>> Handle(GetAllRefApplicationQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<RefApplication> refApplications = _uow.RefApplication.GetRefApplications(request.queryParameters);

            return new PagedList<RefApplicationResult>(_mapper.Map<List<RefApplicationResult>>(refApplications), refApplications.TotalCount);
        }
    }
}
