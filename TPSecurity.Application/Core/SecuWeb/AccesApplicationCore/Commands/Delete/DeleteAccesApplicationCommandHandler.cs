using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Delete
{
    public class DeleteAccesApplicationCommandHandler : IRequestHandler<DeleteAccesApplicationCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteAccesApplicationCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteAccesApplicationCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            AccesApplication accesApplication = _uow.AccesApplication.GetByIdWithReferences(command.id);
            if (accesApplication is null)
                return Errors.NotFound;

            bool canDelete = _uow.AccesApplication.Delete(accesApplication);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
