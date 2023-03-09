using ErrorOr;
using MapsterMapper;
using MediatR;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Errors;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Create;

public class CreateAccesUtilisateurCommandHandler : IRequestHandler<CreateAccesUtilisateurCommand, ErrorOr<AccesUtilisateurResult>>
{
    private readonly IUnitOfWorkGTP _uow;
    private readonly IMapper _mapper;

    public CreateAccesUtilisateurCommandHandler(IUnitOfWorkGTP uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ErrorOr<AccesUtilisateurResult>> Handle(CreateAccesUtilisateurCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_uow.AccesGroupe.GetById(command.IdAccesGroupe) is null)
        {
            return Errors.AccesUtilisateur.AccesGroupeNotFound;
        }

        if (_uow.Utilisateur.GetById(command.IdUtilisateur) is null)
        {
            return Errors.AccesUtilisateur.UtilisateurNotFound;
        }

        if (_uow.AccesUtilisateur.GetByUnicite(command.IdAccesGroupe, command.IdUtilisateur) is not null)
        {
            return Errors.AccesUtilisateur.AccesUtilisateurAlreadyExist;
        }

        ErrorOr<AccesUtilisateur> accesUtilisateur = AccesUtilisateur.Create(command.EstActif, command.IdAccesGroupe, command.IdUtilisateur);

        if (accesUtilisateur.IsError)
            return accesUtilisateur.Errors;

        var dto = _uow.AccesUtilisateur.Create(accesUtilisateur.Value);
        _uow.SaveChanges();

        var created = _uow.AccesUtilisateur.GetById(dto.Id);
        return _mapper.Map<AccesUtilisateurResult>((created, created.GetHashCodeAsString()));
    }
}
