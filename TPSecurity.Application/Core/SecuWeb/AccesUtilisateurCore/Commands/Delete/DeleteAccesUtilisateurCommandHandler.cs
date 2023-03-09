using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Delete
{
    public class DeleteAccesUtilisateurCommandHandler : IRequestHandler<DeleteAccesUtilisateurCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteAccesUtilisateurCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteAccesUtilisateurCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesUtilisateur accesUtilisateur = _uow.AccesUtilisateur.GetById(command.id);
            if (accesUtilisateur is null)
                return Errors.NotFound;

            bool canDelete = _uow.AccesUtilisateur.Delete(accesUtilisateur);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
