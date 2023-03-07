using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

public class UpdateUtilisateurCommandHandler : IRequestHandler<UpdateUtilisateurCommand, ErrorOr<UtilisateurResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public UpdateUtilisateurCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UtilisateurResult>> Handle(UpdateUtilisateurCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Utilisateur utilisateur = _uow.Utilisateur.GetById(command.Id);
        if (utilisateur is null)
            return Errors.NotFound;

        if(command.HashCode != utilisateur.GetHashCodeAsString())
        {
            return Errors.Concurrency;
        }

        var util = _uow.Utilisateur.GetByEmail(command.Email);
        if (util is not null && util.Id != utilisateur.Id)
        {
            return Errors.Utilisateur.DuplicateEmail;
        }

        ErrorOr<Updated> error = utilisateur.Update(command.Nom, command.Prenom, command.Email, command.EstActif);
        
        if (error.IsError)
            return error.Errors;

        var dto = _uow.Utilisateur.Update(utilisateur);
        _uow.SaveChanges();

        var updated = _uow.Utilisateur.GetById(dto.Id);
        return _mapper.Map<UtilisateurResult>((updated, updated.GetHashCodeAsString()));
    }
}
