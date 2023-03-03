using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateRefApplicationCommandHandler : IRequestHandler<UpdateRefApplicationCommand, ErrorOr<RefApplicationResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateRefApplicationCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RefApplicationResult>> Handle(UpdateRefApplicationCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        RefApplication refApplication = _uow.RefApplication.GetById(command.Id);
        if (refApplication is null)
            return Errors.NotFound;

        if(command.HashCode != refApplication.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        ErrorOr<Updated> error = refApplication.Update(command.Libelle, command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.RefApplication.Update(refApplication);
        _uow.SaveChanges();

        var updated = _uow.RefApplication.GetById(dto.Id);
        return _mapper.Map<RefApplicationResult>((updated, updated.GetHashCodeAsString()));

    }
}
