using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetAllRefFonctionnalite;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IRefFonctionnaliteRepository
    {
        RefFonctionnalite? GetById(int id);

        RefFonctionnalite? GetByIdWithReferences(int id);

        RefFonctionnalite? GetByLibelle(string libelle);

        PagedList<RefFonctionnalite> GetRefFonctionnalites(RefFonctionnaliteParameters queryParameters);

        IBaseClass Create(RefFonctionnalite refFonctionnalite);

        IBaseClass Update(RefFonctionnalite refFonctionnalite);

        bool Delete(RefFonctionnalite refFonctionnalite);
    }
}
