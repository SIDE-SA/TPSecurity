using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Enums;
using TPSecurity.Domain.Common.Errors;

public class UpdateRefFonctionnaliteCommandHandler : IRequestHandler<UpdateRefFonctionnaliteCommand, ErrorOr<RefFonctionnaliteResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateRefFonctionnaliteCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RefFonctionnaliteResult>> Handle(UpdateRefFonctionnaliteCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        RefFonctionnalite refFonctionnalite = _uow.RefFonctionnalite.GetById(command.Id);
        if (refFonctionnalite is null)
            return Errors.NotFound;

        if(command.HashCode != refFonctionnalite.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        var refFonct = _uow.RefFonctionnalite.GetByLibelle(command.Libelle);
        if (refFonct is not null && refFonct.Id != refFonctionnalite.Id)
        {
            return Errors.DuplicateLibelle;
        }

        ErrorOr<Updated> error = refFonctionnalite.Update(command.Libelle, command.EstActif, command.EstDefaut);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.RefFonctionnalite.Update(refFonctionnalite);
        _uow.SaveChanges();

        var updated = _uow.RefFonctionnalite.GetById(dto.Id);
        return _mapper.Map<RefFonctionnaliteResult>((updated, updated.GetHashCodeAsString()));
    }
}
