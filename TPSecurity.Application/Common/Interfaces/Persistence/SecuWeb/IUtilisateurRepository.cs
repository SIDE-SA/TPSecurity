using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetAllUtilisateur;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IUtilisateurRepository
    {
        Utilisateur? GetById(int id);

        Utilisateur? GetByIdWithReferences(int id);

        Utilisateur? GetByEmail(string email);

        PagedList<Utilisateur> GetUtilisateurs(UtilisateurParameters queryParameters);

        IBaseClass Create(Utilisateur refModule);

        IBaseClass Update(Utilisateur refModule);

        bool Delete(Utilisateur refModule);
    }
}
