using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetByIdAccesApplication
{
    public class GetByIdAccesApplicationQueryHandler : IRequestHandler<GetByIdAccesApplicationQuery, ErrorOr<AccesApplicationResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdAccesApplicationQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AccesApplicationResult>> Handle(GetByIdAccesApplicationQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesApplication? accesApplications = _uow.AccesApplication.GetById(request.Id);
            if (accesApplications is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<AccesApplicationResult>((accesApplications, accesApplications.GetHashCodeAsString()));
        }
    }
}
