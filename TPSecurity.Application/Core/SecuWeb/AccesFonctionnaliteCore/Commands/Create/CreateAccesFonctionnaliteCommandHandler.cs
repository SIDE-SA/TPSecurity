using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Create;

public class CreateAccesFonctionnaliteCommandHandler : IRequestHandler<CreateAccesFonctionnaliteCommand, ErrorOr<AccesFonctionnaliteResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateAccesFonctionnaliteCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesFonctionnaliteResult>> Handle(CreateAccesFonctionnaliteCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_uow.AccesModule.GetById(command.IdAccesModule) is null)
        {
            return Errors.AccesFonctionnalite.AccesModuleNotFound;
        }

        if (_uow.RefFonctionnalite.GetById(command.IdRefFonctionnalite) is null)
        {
            return Errors.AccesFonctionnalite.RefFonctionnaliteNotFound;
        }

        if (_uow.AccesFonctionnalite.GetByUnicite(command.IdAccesModule, command.IdRefFonctionnalite) is not null)
        {
            return Errors.AccesFonctionnalite.AccesFonctionnaliteAlreadyExist;
        }

        ErrorOr<AccesFonctionnalite> accesFonctionnalite = AccesFonctionnalite.Create(command.EstActif, command.IdAccesModule, command.IdRefFonctionnalite);

        if (accesFonctionnalite.IsError)
            return accesFonctionnalite.Errors;

        var dto = _uow.AccesFonctionnalite.Create(accesFonctionnalite.Value);
        _uow.SaveChanges();

        var created = _uow.AccesFonctionnalite.GetById(dto.Id);
        return _mapper.Map<AccesFonctionnaliteResult>((created, created.GetHashCodeAsString()));
    }
}
