using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetByIdRefApplication
{
    public class GetByIdRefApplicationQueryHandler : IRequestHandler<GetByIdRefApplicationQuery, ErrorOr<RefApplicationResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdRefApplicationQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<RefApplicationResult>> Handle(GetByIdRefApplicationQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            RefApplication? refApplication = _uow.RefApplication.GetById(request.Id);
            if (refApplication is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<RefApplicationResult>((refApplication, refApplication.GetHashCodeAsString()));
        }
    }
}
