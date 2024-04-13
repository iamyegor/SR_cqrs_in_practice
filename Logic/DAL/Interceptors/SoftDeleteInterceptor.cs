using Logic.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Logic.DAL.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
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

        SoftDeleteEnrollments(context);

        return result;
    }

    private void SoftDeleteEnrollments(AppDbContext context)
    {
        List<EntityEntry<Enrollment>> deletedEnrollmentEntries = context
            .ChangeTracker.Entries<Enrollment>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (EntityEntry<Enrollment> deletedEnrollmentEntry in deletedEnrollmentEntries)
        {
            deletedEnrollmentEntry.State = EntityState.Modified;
            Enrollment enrollment = deletedEnrollmentEntry.Entity;
            enrollment.IsDeleted = true;

            if (enrollment.Student == null)
            {
                throw new Exception("Enrollment doesn't have a respective student");
            }

            enrollment.Student.IsSyncNeeded = true;
        }

        if (deletedEnrollmentEntries.Count > 0)
        {
            context.Sync.Single(s => s.Name == "Student").IsSyncRequired = true;
        }
    }
}
