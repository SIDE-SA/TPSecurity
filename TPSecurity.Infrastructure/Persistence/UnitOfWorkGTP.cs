using ErrorOr;
using Microsoft.EntityFrameworkCore;
using TPSecurity.Application.Common.Interfaces.Persistence;
using TPSecurity.Application.Common.Interfaces.Persistence.SecuWeb;
using TPSecurity.Domain.Common.Errors;
using TPSecurity.Infrastructure.Interfaces;
using TPSecurity.Infrastructure.Persistence.Repositories.SecuWeb;

namespace TPSecurity.Infrastructure.Persistence
{
    public class UnitOfWorkGTP : IUnitOfWorkGTP, IDisposable
    {
        private readonly DbContext _context;

        public IRefApplicationRepository RefApplication { get; private set; }

        public IRefModuleRepository RefModule { get; private set; }

        public IAccesGroupeRepository AccesGroupe { get; private set; }

        public UnitOfWorkGTP(ApplicationContextGTP context,
                             ITenantService tenantService)
        {
            _context = context;

            RefApplication = new RefApplicationRepository(context);
            RefModule = new RefModuleRepository(context);
            AccesGroupe = new AccesGroupeRepository(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public ErrorOr<int> SaveChanges()
        {           
            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                CancelChanges();
                return Errors.DBError;
            }
            
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void CancelChanges()
        {
            foreach(var entry in _context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }
        }
    }
}
