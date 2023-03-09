using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetByIdAccesUtilisateur;

public record GetByIdAccesUtilisateurQuery(int Id) : IRequest<ErrorOr<AccesUtilisateurResult>>;