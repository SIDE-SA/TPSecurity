using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Services.GeneralConcept;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Create;

public class CreateAccesGroupeCommandHandler : IRequestHandler<CreateAccesGroupeCommand, ErrorOr<AccesGroupeResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;
    private readonly IGeneralConceptService _generalConceptService;

    public CreateAccesGroupeCommandHandler(IUnitOfWorkGTP uow, IMapper mapper, IGeneralConceptService generalConceptService)
    {
        _uow = uow;
        _mapper = mapper;
        _generalConceptService = generalConceptService;
    }

    public async Task<ErrorOr<AccesGroupeResult>> Handle(CreateAccesGroupeCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_uow.AccesGroupe.GetByLibelle(command.Libelle) is not null)
        {
            return Errors.DuplicateLibelle;
        }

        bool societyExist = await _generalConceptService.Societe.Exist(command.IdSociete);
        if (!societyExist)
        {
            return Errors.AccesGroupe.SocietyNotFound;
        }

        ErrorOr<AccesGroupe> accesGroupe = AccesGroupe.Create(command.Libelle, command.EstActif, command.EstGroupeSpecial, command.IdSociete);

        if (accesGroupe.IsError)
            return accesGroupe.Errors;

        var dto = _uow.AccesGroupe.Create(accesGroupe.Value);
        _uow.SaveChanges();

        var created = _uow.AccesGroupe.GetById(dto.Id);
        return _mapper.Map<AccesGroupeResult>((created, created.GetHashCodeAsString()));
    }
}
