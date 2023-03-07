using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateAccesFonctionnaliteCommandHandler : IRequestHandler<UpdateAccesFonctionnaliteCommand, ErrorOr<AccesFonctionnaliteResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateAccesFonctionnaliteCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesFonctionnaliteResult>> Handle(UpdateAccesFonctionnaliteCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        AccesFonctionnalite accesFonctionnalite = _uow.AccesFonctionnalite.GetById(command.Id);
        if (accesFonctionnalite is null)
            return Errors.NotFound;

        if(command.HashCode != accesFonctionnalite.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        ErrorOr<Updated> error = accesFonctionnalite.Update(command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.AccesFonctionnalite.Update(accesFonctionnalite);
        _uow.SaveChanges();

        var updated = _uow.AccesFonctionnalite.GetById(dto.Id);
        return _mapper.Map<AccesFonctionnaliteResult>((updated, updated.GetHashCodeAsString()));
    }
}
