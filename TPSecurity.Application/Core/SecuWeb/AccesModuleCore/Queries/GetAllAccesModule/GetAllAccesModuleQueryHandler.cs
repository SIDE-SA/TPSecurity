using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetAllAccesModule
{
    public class GetAllAccesModuleQueryHandler : IRequestHandler<GetAllAccesModuleQuery, ErrorOr<PagedList<AccesModuleResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllAccesModuleQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<AccesModuleResult>>> Handle(GetAllAccesModuleQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<AccesModule> accesModule = _uow.AccesModule.GetAccesModules(request.queryParameters);

            return new PagedList<AccesModuleResult>(_mapper.Map<List<AccesModuleResult>>(accesModule), accesModule.TotalCount);
        }
    }
}
