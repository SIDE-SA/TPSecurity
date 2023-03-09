using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetByIdAccesUtilisateur
{
    public class GetByIdAccesUtilisateurQueryHandler : IRequestHandler<GetByIdAccesUtilisateurQuery, ErrorOr<AccesUtilisateurResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdAccesUtilisateurQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AccesUtilisateurResult>> Handle(GetByIdAccesUtilisateurQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesUtilisateur? accesUtilisateurs = _uow.AccesUtilisateur.GetById(request.Id);
            if (accesUtilisateurs is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<AccesUtilisateurResult>((accesUtilisateurs, accesUtilisateurs.GetHashCodeAsString()));
        }
    }
}
