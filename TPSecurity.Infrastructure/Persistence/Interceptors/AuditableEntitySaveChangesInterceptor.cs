using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TPSecurity.Application.Common.Interfaces.Services;
using TPSecurity.Infrastructure.Models;

namespace TPSecurity.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{    
    private readonly IDateTimeProvider _dateTime;
    private readonly IUserRequestService _userRequestService;

    public AuditableEntitySaveChangesInterceptor(
        IDateTimeProvider dateTime, 
        IUserRequestService userRequestService)
    {
        _dateTime = dateTime;
        _userRequestService = userRequestService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseClass>())
        {          
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreation = _dateTime.Now;
                entry.Entity.UserCreation = _userRequestService.UserName;
            }

            entry.Entity.DateModification = _dateTime.Now;
            entry.Entity.UserModification = _userRequestService.UserName;
        }       
    }
}