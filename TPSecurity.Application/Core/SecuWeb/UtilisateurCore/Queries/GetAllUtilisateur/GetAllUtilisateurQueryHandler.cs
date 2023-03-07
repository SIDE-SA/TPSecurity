using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetAllUtilisateur
{
    public class GetAllRefModukeQueryHandler : IRequestHandler<GetAllUtilisateurQuery, ErrorOr<PagedList<UtilisateurResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllRefModukeQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<UtilisateurResult>>> Handle(GetAllUtilisateurQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<Utilisateur> utilisateurs = _uow.Utilisateur.GetUtilisateurs(request.queryParameters);

            return new PagedList<UtilisateurResult>(_mapper.Map<List<UtilisateurResult>>(utilisateurs), utilisateurs.TotalCount);
        }
    }
}
