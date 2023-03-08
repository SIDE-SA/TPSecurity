using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Queries.GetAllAccesByIdUtilisateur
{
    public class GetAllAccesByIdUtilisateurQueryHandler : IRequestHandler<GetAllAccesByIdUtilisateurQuery, ErrorOr<List<UtilisateurAccesResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllAccesByIdUtilisateurQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<List<UtilisateurAccesResult>>> Handle(GetAllAccesByIdUtilisateurQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            IEnumerable<AccesGroupe>? listAccesGroupe = _uow.AccesGroupe.GetByIdUtilisateur(request.Id);
            if (listAccesGroupe is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<List<UtilisateurAccesResult>>(listAccesGroupe);
        }
    }
}
