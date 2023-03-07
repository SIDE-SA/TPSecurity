using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetByIdUtilisateur
{
    public class GetByIdUtilisateurQueryHandler : IRequestHandler<GetByIdUtilisateurQuery, ErrorOr<UtilisateurResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdUtilisateurQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UtilisateurResult>> Handle(GetByIdUtilisateurQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Utilisateur? utilisateur = _uow.Utilisateur.GetById(request.Id);
            if (utilisateur is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<UtilisateurResult>((utilisateur, utilisateur.GetHashCodeAsString()));
        }
    }
}
