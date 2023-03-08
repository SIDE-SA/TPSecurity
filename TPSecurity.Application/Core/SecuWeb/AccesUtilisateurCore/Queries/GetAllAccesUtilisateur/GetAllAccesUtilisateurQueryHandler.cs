using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetAllAccesUtilisateur
{
    public class GetAllAccesUtilisateurQueryHandler : IRequestHandler<GetAllAccesUtilisateurQuery, ErrorOr<PagedList<AccesUtilisateurResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllAccesUtilisateurQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<AccesUtilisateurResult>>> Handle(GetAllAccesUtilisateurQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<AccesUtilisateur> accesUtilisateur = _uow.AccesUtilisateur.GetAccesUtilisateurs(request.queryParameters);

            return new PagedList<AccesUtilisateurResult>(_mapper.Map<List<AccesUtilisateurResult>>(accesUtilisateur), accesUtilisateur.TotalCount);
        }
    }
}
