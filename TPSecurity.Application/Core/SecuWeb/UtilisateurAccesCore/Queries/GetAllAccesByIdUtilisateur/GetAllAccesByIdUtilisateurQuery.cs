using ErrorOr;
using MediatR;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Queries.GetAllAccesByIdUtilisateur;

public record GetAllAccesByIdUtilisateurQuery(int Id) : IRequest<ErrorOr<List<UtilisateurAccesResult>>>;