using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetAllAccesModule;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IAccesModuleRepository
    {

        AccesModule? GetById(int id);

        AccesModule? GetByUnicite(int idAccesApplication, int idRefModule);

        AccesModule? GetByIdWithReferences(int id);

        PagedList<AccesModule> GetAccesModules(AccesModuleParameters queryParameters);

        IBaseClass Create(AccesModule accesModule);

        IBaseClass Update(AccesModule accesModule);

        bool Delete(AccesModule accesModule);
    }
}
