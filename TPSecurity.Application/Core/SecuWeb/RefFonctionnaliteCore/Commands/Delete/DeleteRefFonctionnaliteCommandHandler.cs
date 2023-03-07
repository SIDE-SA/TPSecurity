using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Delete
{
    public class DeleteRefFonctionnaliteCommandHandler : IRequestHandler<DeleteRefFonctionnaliteCommand, ErrorOr<Deleted>>
    {
        private readonly IUnitOfWorkGTP _uow;
        private readonly IMapper _mapper;

        public DeleteRefFonctionnaliteCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteRefFonctionnaliteCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            RefFonctionnalite refFonctionnalite = _uow.RefFonctionnalite.GetByIdWithReferences(command.id);
            if (refFonctionnalite is null)
                return Errors.NotFound;

            bool canDelete = _uow.RefFonctionnalite.Delete(refFonctionnalite);
            if (!canDelete)
                return Errors.InUse;

            _uow.SaveChanges();
            return Result.Deleted;
        }
    }
}
