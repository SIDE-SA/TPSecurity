using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateRefModuleCommandHandler : IRequestHandler<UpdateRefModuleCommand, ErrorOr<RefModuleResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateRefModuleCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RefModuleResult>> Handle(UpdateRefModuleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        RefModule refModule = _uow.RefModule.GetById(command.Id);
        if (refModule is null)
            return Errors.NotFound;

        if(command.HashCode != refModule.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        var refMod = _uow.RefModule.GetByLibelle(command.Libelle);
        if (refMod is not null && refMod.Id != refModule.Id)
        {
            return Errors.DuplicateLibelle;
        }

        ErrorOr<Updated> error = refModule.Update(command.Libelle, command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.RefModule.Update(refModule);
        _uow.SaveChanges();

        var updated = _uow.RefModule.GetById(dto.Id);
        return _mapper.Map<RefModuleResult>((updated, updated.GetHashCodeAsString()));
    }
}
