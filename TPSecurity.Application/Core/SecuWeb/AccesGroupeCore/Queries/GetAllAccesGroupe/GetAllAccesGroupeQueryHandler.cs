using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetAllAccesGroupe
{
    public class GetAllAccesGroupeQueryHandler : IRequestHandler<GetAllAccesGroupeQuery, ErrorOr<PagedList<AccesGroupeResult>>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public GetAllAccesGroupeQueryHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PagedList<AccesGroupeResult>>> Handle(GetAllAccesGroupeQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            PagedList<AccesGroupe> accesGroupes = _uow.AccesGroupe.GetAccesGroupes(request.queryParameters);

            return new PagedList<AccesGroupeResult>(_mapper.Map<List<AccesGroupeResult>>(accesGroupes), accesGroupes.TotalCount);
        }
    }
}
