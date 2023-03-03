using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetByIdRefModule
{
    public class GetByIdRefModuleQueryHandler : IRequestHandler<GetByIdRefModuleQuery, ErrorOr<RefModuleResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdRefModuleQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<RefModuleResult>> Handle(GetByIdRefModuleQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            RefModule? refApplication = _uow.RefModule.GetById(request.Id);
            if (refApplication is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<RefModuleResult>((refApplication, refApplication.GetHashCodeAsString()));
        }
    }
}
