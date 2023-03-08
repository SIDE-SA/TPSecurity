using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateAccesModuleCommandHandler : IRequestHandler<UpdateAccesModuleCommand, ErrorOr<AccesModuleResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateAccesModuleCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesModuleResult>> Handle(UpdateAccesModuleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        AccesModule accesModule = _uow.AccesModule.GetById(command.Id);
        if (accesModule is null)
            return Errors.NotFound;

        if(command.HashCode != accesModule.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        ErrorOr<Updated> error = accesModule.Update(command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.AccesModule.Update(accesModule);
        _uow.SaveChanges();

        var updated = _uow.AccesModule.GetById(dto.Id);
        return _mapper.Map<AccesModuleResult>((updated, updated.GetHashCodeAsString()));
    }
}
