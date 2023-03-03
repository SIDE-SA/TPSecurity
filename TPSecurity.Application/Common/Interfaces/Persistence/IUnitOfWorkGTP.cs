using ErrorOr;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWorkGTP
    {      
        
        IRefApplicationRepository RefApplication { get; }

        Task SaveChangesAsync();

        ErrorOr<int> SaveChanges();

        void CancelChanges();

        void Dispose();
    }
}
