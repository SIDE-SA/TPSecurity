using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Create;

public class CreateRefModuleCommandHandler : IRequestHandler<CreateRefModuleCommand, ErrorOr<RefModuleResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateRefModuleCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RefModuleResult>> Handle(CreateRefModuleCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_uow.RefModule.GetByLibelle(command.Libelle) is not null)
        {
            return Errors.DuplicateLibelle;
        }

        RefApplication refApp = _uow.RefApplication.GetById(command.IdRefApplication);
        if (refApp is null)
        {
            return Errors.RefModule.RefApplicationNotFound;
        }

        ErrorOr<RefModule> refModule = RefModule.Create(command.Libelle, command.EstActif, refApp);

        if (refModule.IsError)
            return refModule.Errors;

        var dto = _uow.RefModule.Create(refModule.Value);
        _uow.SaveChanges();

        var created = _uow.RefModule.GetById(dto.Id);
        return _mapper.Map<RefModuleResult>((created, created.GetHashCodeAsString()));
    }
}
