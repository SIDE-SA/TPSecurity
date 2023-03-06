using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Delete
{
    public class DeleteRefModuleCommandHandler : IRequestHandler<DeleteRefModuleCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteRefModuleCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteRefModuleCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            RefModule refModule = _uow.RefModule.GetByIdWithReferences(command.id);
            if (refModule is null)
                return Errors.NotFound;

            bool canDelete = _uow.RefModule.Delete(refModule);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
