using CSharpFunctionalExtensions;
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
        List<EntityEntry<Enrollment>> entriesOfDeletedEnrollments = context
            .ChangeTracker.Entries<Enrollment>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entryOfDeletedEnrollment in entriesOfDeletedEnrollments)
        {
            Enrollment enrollment = entryOfDeletedEnrollment.Entity;
            entryOfDeletedEnrollment.State = EntityState.Modified;
            enrollment.IsDeleted = true;

            Student affectedStudents = context
                .ChangeTracker.Entries<Student>()
                .Where(s => s.Entity.Enrollments.Any(e => e.Id == enrollment.Id))
                .Select(e => e.Entity)
                .Single();

            affectedStudents.IsSyncNeeded = true;
        }
    }
}
