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

        RefModule refApplication = _uow.RefModule.GetById(command.Id);
        if (refApplication is null)
            return Errors.NotFound;

        if(command.HashCode != refApplication.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        var refAppli = _uow.RefModule.GetByLibelle(command.Libelle);
        if (refAppli is not null && refAppli.Id != refApplication.Id)
        {
            return Errors.DuplicateLibelle;
        }

        ErrorOr<Updated> error = refApplication.Update(command.Libelle, command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.RefModule.Update(refApplication);
        _uow.SaveChanges();

        var updated = _uow.RefModule.GetById(dto.Id);
        return _mapper.Map<RefModuleResult>((updated, updated.GetHashCodeAsString()));
    }
}
