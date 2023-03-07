using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Create;

public class CreateAccesModuleCommandHandler : IRequestHandler<CreateAccesModuleCommand, ErrorOr<AccesModuleResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateAccesModuleCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesModuleResult>> Handle(CreateAccesModuleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_uow.AccesApplication.GetById(command.IdAccesApplication) is null)
        {
            return Errors.AccesModule.AccesApplicationNotFound;
        }

        if (_uow.RefModule.GetById(command.IdRefModule) is null)
        {
            return Errors.AccesModule.RefModuleNotFound;
        }

        if (_uow.AccesModule.GetByUnicite(command.IdAccesApplication, command.IdRefModule) is not null)
        {
            return Errors.AccesModule.AccesModuleAlreadyExist;
        }

        ErrorOr<AccesModule> accesModule = AccesModule.Create(command.EstActif, command.IdAccesApplication, command.IdRefModule);

        if (accesModule.IsError)
            return accesModule.Errors;

        var dto = _uow.AccesModule.Create(accesModule.Value);
        _uow.SaveChanges();

        var created = _uow.AccesModule.GetById(dto.Id);
        return _mapper.Map<AccesModuleResult>((created, created.GetHashCodeAsString()));
    }
}
