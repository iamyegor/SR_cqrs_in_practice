using Logic.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Logic.DAL.Interceptors;

public class SyncInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        if (eventData.Context is not AppDbContext context)
        {
            return result;
        }

        RaiseSyncInStudent(context);

        return result;
    }

    private void RaiseSyncInStudent(AppDbContext context)
    {
        bool isSyncRaised = false;
        foreach (var studentEntry in context.ChangeTracker.Entries<Student>())
        {
            if (
                studentEntry.State == EntityState.Added
                || studentEntry.State == EntityState.Modified
            )
            {
                studentEntry.Entity.IsSyncNeeded = true;
                isSyncRaised = true;
            }
        }

        if (isSyncRaised)
        {
            context.Sync.Single(s => s.Name == "Student").IsSyncRequired = true;
        }
    }
}
