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

        RaiseSyncFlags(context);

        return result;
    }

    private void RaiseSyncFlags(AppDbContext context)
    {
        bool isSyncRaised = false;
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (
                entry.Entity is Student student
                && (entry.State == EntityState.Modified || entry.State == EntityState.Added)
            )
            {
                student.IsSyncNeeded = true;
                isSyncRaised = true;
            }
        }

        if (isSyncRaised)
        {
            context.Sync.Single(s => s.Name == "Student").IsSyncRequired = true;
        }
    }
}
