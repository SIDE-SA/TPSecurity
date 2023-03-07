using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Delete
{
    public class DeleteAccesGroupeCommandHandler : IRequestHandler<DeleteAccesGroupeCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteAccesGroupeCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteAccesGroupeCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesGroupe accesGroupe = _uow.AccesGroupe.GetByIdWithReferences(command.id);
            if (accesGroupe is null)
                return Errors.NotFound;

            bool canDelete = _uow.AccesGroupe.Delete(accesGroupe);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
