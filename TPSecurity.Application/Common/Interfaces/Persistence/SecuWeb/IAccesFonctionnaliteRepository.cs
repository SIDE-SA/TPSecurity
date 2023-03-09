using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetAllAccesFonctionnalite;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IAccesFonctionnaliteRepository
    {
        AccesFonctionnalite? GetById(int id);

        AccesFonctionnalite? GetByUnicite(int idAccesApplication, int idRefFonctionnalite);

        PagedList<AccesFonctionnalite> GetAccesFonctionnalites(AccesFonctionnaliteParameters queryParameters);

        IBaseClass Create(AccesFonctionnalite accesFonctionnalite);

        IBaseClass Update(AccesFonctionnalite accesFonctionnalite);

        bool Delete(AccesFonctionnalite accesFonctionnalite);
    }
}
