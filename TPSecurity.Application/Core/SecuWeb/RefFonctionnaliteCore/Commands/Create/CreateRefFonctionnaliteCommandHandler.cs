using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Create;

public class CreateRefFonctionnaliteCommandHandler : IRequestHandler<CreateRefFonctionnaliteCommand, ErrorOr<RefFonctionnaliteResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateRefFonctionnaliteCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RefFonctionnaliteResult>> Handle(CreateRefFonctionnaliteCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        RefFonctionnalite refAppli = _uow.RefFonctionnalite.GetByLibelle(command.Libelle);
        if (refAppli is not null)
        {
            return Errors.DuplicateLibelle;
        }

        ErrorOr<RefFonctionnalite> refFonctionnalite = RefFonctionnalite.Create(command.Libelle, command.EstActif, command.EstDefaut, command.Permission, command.IdRefModule);

        if (refFonctionnalite.IsError)
            return refFonctionnalite.Errors;

        var dto = _uow.RefFonctionnalite.Create(refFonctionnalite.Value);
        _uow.SaveChanges();

        var created = _uow.RefFonctionnalite.GetById(dto.Id);
        return _mapper.Map<RefFonctionnaliteResult>((created, created.GetHashCodeAsString()));
    }
}
