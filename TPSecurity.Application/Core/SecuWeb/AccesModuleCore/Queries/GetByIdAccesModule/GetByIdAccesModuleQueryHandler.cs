using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetByIdAccesModule
{
    public class GetByIdAccesModuleQueryHandler : IRequestHandler<GetByIdAccesModuleQuery, ErrorOr<AccesModuleResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdAccesModuleQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AccesModuleResult>> Handle(GetByIdAccesModuleQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesModule? accesModules = _uow.AccesModule.GetById(request.Id);
            if (accesModules is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<AccesModuleResult>((accesModules, accesModules.GetHashCodeAsString()));
        }
    }
}
