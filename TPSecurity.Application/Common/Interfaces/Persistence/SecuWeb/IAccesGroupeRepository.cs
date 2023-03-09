using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetAllAccesGroupe;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IAccesGroupeRepository
    {
        AccesGroupe? GetById(int id);

        AccesGroupe? GetByIdWithReferences(int id);

        IEnumerable<AccesGroupe>? GetByIdUtilisateur(int idUtilisateur);

        AccesGroupe? GetByLibelle(string libelle);

        PagedList<AccesGroupe> GetAccesGroupes(AccesGroupeParameters queryParameters);

        IBaseClass Create(AccesGroupe accesGroupe);

        IBaseClass Update(AccesGroupe accesGroupe);

        bool Delete(AccesGroupe accesGroupe);
    }
}
