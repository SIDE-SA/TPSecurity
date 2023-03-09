using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Delete
{
    public class DeleteAccesFonctionnaliteCommandHandler : IRequestHandler<DeleteAccesFonctionnaliteCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteAccesFonctionnaliteCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteAccesFonctionnaliteCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesFonctionnalite accesFonctionnalite = _uow.AccesFonctionnalite.GetById(command.id);
            if (accesFonctionnalite is null)
                return Errors.NotFound;

            bool canDelete = _uow.AccesFonctionnalite.Delete(accesFonctionnalite);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
