using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateAccesApplicationCommandHandler : IRequestHandler<UpdateAccesApplicationCommand, ErrorOr<AccesApplicationResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateAccesApplicationCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesApplicationResult>> Handle(UpdateAccesApplicationCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        AccesApplication accesApplication = _uow.AccesApplication.GetById(command.Id);
        if (accesApplication is null)
            return Errors.NotFound;

        if(command.HashCode != accesApplication.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        ErrorOr<Updated> error = accesApplication.Update(command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.AccesApplication.Update(accesApplication);
        _uow.SaveChanges();

        var updated = _uow.AccesApplication.GetById(dto.Id);
        return _mapper.Map<AccesApplicationResult>((updated, updated.GetHashCodeAsString()));
    }
}
