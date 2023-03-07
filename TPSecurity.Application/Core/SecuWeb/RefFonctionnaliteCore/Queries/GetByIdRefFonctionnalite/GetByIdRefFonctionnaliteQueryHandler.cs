using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetByIdRefFonctionnalite
{
    public class GetByIdRefFonctionnaliteQueryHandler : IRequestHandler<GetByIdRefFonctionnaliteQuery, ErrorOr<RefFonctionnaliteResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdRefFonctionnaliteQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<RefFonctionnaliteResult>> Handle(GetByIdRefFonctionnaliteQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            RefFonctionnalite? refFonctionnalite = _uow.RefFonctionnalite.GetById(request.Id);
            if (refFonctionnalite is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<RefFonctionnaliteResult>((refFonctionnalite, refFonctionnalite.GetHashCodeAsString()));
        }
    }
}
