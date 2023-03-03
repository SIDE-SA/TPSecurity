using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetAllRefApplication;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb
{
    public interface IRefApplicationRepository
    {

        RefApplication? GetById(int id);

        RefApplication? GetByIdWithReferences(int id);

        PagedList<RefApplication> GetRefApplications(RefApplicationParameters queryParameters);

        IBaseClass Create(RefApplication refApplication);

        IBaseClass Update(RefApplication refApplication);

        bool Delete(RefApplication refApplication);
    }
}
