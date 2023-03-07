using ErrorOr;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;

namespace TPSecurity.Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWorkGTP
    {      
        
        IRefApplicationRepository RefApplication { get; }

        IRefFonctionnaliteRepository RefFonctionnalite { get; }

        IRefModuleRepository RefModule { get; }

        IAccesGroupeRepository AccesGroupe { get; }

        IAccesApplicationRepository AccesApplication { get; }

        IAccesModuleRepository AccesModule{ get; }

        IAccesFonctionnaliteRepository AccesFonctionnalite { get; }

        Task SaveChangesAsync();

        ErrorOr<int> SaveChanges();

        void CancelChanges();

        void Dispose();
    }
}
