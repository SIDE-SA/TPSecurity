using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateAccesGroupeCommandHandler : IRequestHandler<UpdateAccesGroupeCommand, ErrorOr<AccesGroupeResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateAccesGroupeCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesGroupeResult>> Handle(UpdateAccesGroupeCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        AccesGroupe accesGroupe = _uow.AccesGroupe.GetById(command.Id);
        if (accesGroupe is null)
            return Errors.NotFound;

        if(command.HashCode != accesGroupe.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        var accesGrp = _uow.AccesGroupe.GetByLibelle(command.Libelle);
        if (accesGrp is not null && accesGrp.Id != accesGroupe.Id)
        {
            return Errors.DuplicateLibelle;
        }

        ErrorOr<Updated> error = accesGroupe.Update(command.Libelle, command.EstActif, command.EstGroupeSpecial);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.AccesGroupe.Update(accesGroupe);
        _uow.SaveChanges();

        var updated = _uow.AccesGroupe.GetById(dto.Id);
        return _mapper.Map<AccesGroupeResult>((updated, updated.GetHashCodeAsString()));
    }
}
