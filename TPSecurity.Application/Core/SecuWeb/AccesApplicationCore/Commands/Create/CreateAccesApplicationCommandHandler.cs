using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Services.GeneralConcept;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Create;

public class CreateAccesApplicationCommandHandler : IRequestHandler<CreateAccesApplicationCommand, ErrorOr<AccesApplicationResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateAccesApplicationCommandHandler(IUnitOfWorkGTP uow, IMapper mapper, IGeneralConceptService generalConceptService)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesApplicationResult>> Handle(CreateAccesApplicationCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        ErrorOr<AccesApplication> accesApplication = AccesApplication.Create(command.EstActif, command.IdAccesGroupe, command.IdRefApplication);

        if (accesApplication.IsError)
            return accesApplication.Errors;

        var dto = _uow.AccesApplication.Create(accesApplication.Value);
        _uow.SaveChanges();

        var created = _uow.AccesApplication.GetById(dto.Id);
        return _mapper.Map<AccesApplicationResult>((created, created.GetHashCodeAsString()));
    }
}
