using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Create;

public class CreateRefApplicationCommandHandler : IRequestHandler<CreateRefApplicationCommand, ErrorOr<RefApplicationResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateRefApplicationCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RefApplicationResult>> Handle(CreateRefApplicationCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        ErrorOr<RefApplication> refApplication = RefApplication.Create(command.Libelle, command.EstActif);

        if (refApplication.IsError)
            return refApplication.Errors;

        var dto = _uow.RefApplication.Create(refApplication.Value);
        _uow.SaveChanges();

        var created = _uow.RefApplication.GetById(dto.Id);
        return _mapper.Map<RefApplicationResult>((created, created.GetHashCodeAsString()));

    }
}
