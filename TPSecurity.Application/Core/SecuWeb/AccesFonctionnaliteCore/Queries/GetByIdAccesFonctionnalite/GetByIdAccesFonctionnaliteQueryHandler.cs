using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetByIdAccesFonctionnalite
{
    public class GetByIdAccesFonctionnaliteQueryHandler : IRequestHandler<GetByIdAccesFonctionnaliteQuery, ErrorOr<AccesFonctionnaliteResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdAccesFonctionnaliteQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AccesFonctionnaliteResult>> Handle(GetByIdAccesFonctionnaliteQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesFonctionnalite? accesFonctionnalites = _uow.AccesFonctionnalite.GetById(request.Id);
            if (accesFonctionnalites is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<AccesFonctionnaliteResult>((accesFonctionnalites, accesFonctionnalites.GetHashCodeAsString()));
        }
    }
}
