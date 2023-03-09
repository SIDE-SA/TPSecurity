using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IAccesApplicationRepository
    {
        AccesApplication? GetById(int id);

        AccesApplication? GetByUnicite(int idAccesGroupe, int idRefApplication);

        AccesApplication? GetByIdWithReferences(int id);

        PagedList<AccesApplication> GetAccesApplications(AccesApplicationParameters queryParameters);

        IBaseClass Create(AccesApplication accesApplication);

        IBaseClass Update(AccesApplication accesApplication);

        bool Delete(AccesApplication accesApplication);
    }
}
