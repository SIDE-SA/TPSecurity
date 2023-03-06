using ErrorOr;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWorkGTP
    {      
        
        IRefApplicationRepository RefApplication { get; }

        IRefModuleRepository RefModule { get; }

        Task SaveChangesAsync();

        ErrorOr<int> SaveChanges();

        void CancelChanges();

        void Dispose();
    }
}
