using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IAccesApplicationRepository
    {

        AccesApplication? GetById(int id);

        AccesApplication? GetByIdWithReferences(int id);

        PagedList<AccesApplication> GetAccesApplications(AccesApplicationParameters queryParameters);

        IBaseClass Create(AccesApplication accesGroupe);

        IBaseClass Update(AccesApplication accesGroupe);

        bool Delete(AccesApplication accesGroupe);
    }
}
