using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetAllAccesUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IAccesUtilisateurRepository
    {

        AccesUtilisateur? GetById(int id);

        AccesUtilisateur? GetByUnicite(int idAccesGroupe, int idUtilisateur);

        PagedList<AccesUtilisateur> GetAccesUtilisateurs(AccesUtilisateurParameters queryParameters);

        IBaseClass Create(AccesUtilisateur accesUtilisateur);

        IBaseClass Update(AccesUtilisateur accesUtilisateur);

        bool Delete(AccesUtilisateur accesUtilisateur);
    }
}
