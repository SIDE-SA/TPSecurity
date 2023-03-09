using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateAccesUtilisateurCommandHandler : IRequestHandler<UpdateAccesUtilisateurCommand, ErrorOr<AccesUtilisateurResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateAccesUtilisateurCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesUtilisateurResult>> Handle(UpdateAccesUtilisateurCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        AccesUtilisateur accesUtilisateur = _uow.AccesUtilisateur.GetById(command.Id);
        if (accesUtilisateur is null)
            return Errors.NotFound;

        if(command.HashCode != accesUtilisateur.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        ErrorOr<Updated> error = accesUtilisateur.Update(command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.AccesUtilisateur.Update(accesUtilisateur);
        _uow.SaveChanges();

        var updated = _uow.AccesUtilisateur.GetById(dto.Id);
        return _mapper.Map<AccesUtilisateurResult>((updated, updated.GetHashCodeAsString()));
    }
}
