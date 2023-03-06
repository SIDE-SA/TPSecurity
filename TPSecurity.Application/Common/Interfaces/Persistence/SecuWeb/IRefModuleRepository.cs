using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetAllRefModule;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IRefModuleRepository
    {

        RefModule? GetById(int id);

        RefModule? GetByIdWithReferences(int id);

        RefModule? GetByLibelle(string libelle);

        PagedList<RefModule> GetRefModules(RefModuleParameters queryParameters);

        IBaseClass Create(RefModule refModule);

        IBaseClass Update(RefModule refModule);

        bool Delete(RefModule refModule);
    }
}
