using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Delete
{
    public class DeleteAccesModuleCommandHandler : IRequestHandler<DeleteAccesModuleCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteAccesModuleCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteAccesModuleCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesModule accesModule = _uow.AccesModule.GetByIdWithReferences(command.id);
            if (accesModule is null)
                return Errors.NotFound;

            bool canDelete = _uow.AccesModule.Delete(accesModule);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
