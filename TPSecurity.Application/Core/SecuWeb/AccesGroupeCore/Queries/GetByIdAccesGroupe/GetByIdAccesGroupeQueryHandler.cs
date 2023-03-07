using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetByIdAccesGroupe
{
    public class GetByIdAccesGroupeQueryHandler : IRequestHandler<GetByIdAccesGroupeQuery, ErrorOr<AccesGroupeResult>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetByIdAccesGroupeQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<AccesGroupeResult>> Handle(GetByIdAccesGroupeQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesGroupe? accesGroupe = _uow.AccesGroupe.GetById(request.Id);
            if (accesGroupe is null)
            {
                return Errors.NotFound;
            }

            return _mapper.Map<AccesGroupeResult>((accesGroupe, accesGroupe.GetHashCodeAsString()));
        }
    }
}
