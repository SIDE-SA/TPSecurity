using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetAllRefModule
{
    public class GetAllRefModuleQueryHandler : IRequestHandler<GetAllRefModuleQuery, ErrorOr<PagedList<RefModuleResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllRefModuleQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<RefModuleResult>>> Handle(GetAllRefModuleQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<RefModule> refApplications = _uow.RefModule.GetRefModules(request.queryParameters);

            return new PagedList<RefModuleResult>(_mapper.Map<List<RefModuleResult>>(refApplications), refApplications.TotalCount);
        }
    }
}
