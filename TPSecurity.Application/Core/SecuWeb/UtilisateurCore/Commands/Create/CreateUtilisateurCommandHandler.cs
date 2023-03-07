using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Create;

public class CreateUtilisateurCommandHandler : IRequestHandler<CreateUtilisateurCommand, ErrorOr<UtilisateurResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateUtilisateurCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UtilisateurResult>> Handle(CreateUtilisateurCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_uow.Utilisateur.GetByEmail(command.Email) is not null)
        {
            return Errors.Utilisateur.DuplicateEmail;
        }

        ErrorOr<Utilisateur> utilisateur = Utilisateur.Create(command.Nom, command.Prenom, command.Email, command.EstActif);

        if (utilisateur.IsError)
            return utilisateur.Errors;

        var dto = _uow.Utilisateur.Create(utilisateur.Value);
        _uow.SaveChanges();

        var created = _uow.Utilisateur.GetById(dto.Id);
        return _mapper.Map<UtilisateurResult>((created, created.GetHashCodeAsString()));
    }
}
