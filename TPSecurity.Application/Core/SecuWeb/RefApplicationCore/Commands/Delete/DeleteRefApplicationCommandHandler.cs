using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Delete
{
    public class DeleteRefApplicationCommandHandler : IRequestHandler<DeleteRefApplicationCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteRefApplicationCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteRefApplicationCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            RefApplication refApplication = _uow.RefApplication.GetByIdWithReferences(command.id);
            if (refApplication is null)
                return Errors.NotFound;

            bool canDelete = _uow.RefApplication.Delete(refApplication);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
