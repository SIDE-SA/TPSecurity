using ErrorOr;
using MediatR;
using TPSecurity.Application.Common;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Common;

namespace TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetAllAccesUtilisateur;

public record GetAllAccesUtilisateurQuery(AccesUtilisateurParameters queryParameters) : IRequest<ErrorOr<PagedList<AccesUtilisateurResult>>>;