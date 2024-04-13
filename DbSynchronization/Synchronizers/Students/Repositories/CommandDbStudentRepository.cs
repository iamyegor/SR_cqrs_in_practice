using DbSynchronization.Synchronizers.Students.Models;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbStudentRepository
{
    public IReadOnlyList<StudentInCommandDb> GetForSyncAndResetFlags(NpgsqlTransaction transaction)
    {
        (List<int> studentIds, List<int> enrollmentIds) = LockStudentsAndEnrollments(transaction);

        IReadOnlyList<StudentInCommandDb> students = GetStudentsForSync(studentIds, transaction);

        DeleteSoftDeletedEnrollments(enrollmentIds, transaction);
        ResetSyncFlags(studentIds, transaction);

        return students;
    }

    private (List<int> studentIds, List<int> enrollmentIds) LockStudentsAndEnrollments(
        NpgsqlTransaction transaction
    )
    {
        throw new NotImplementedException();
    }

    private void DeleteSoftDeletedEnrollments(
        List<int> enrollmentIds,
        NpgsqlTransaction transaction
    )
    {
        throw new NotImplementedException();
    }

    private IReadOnlyList<StudentInCommandDb> GetStudentsForSync(
        List<int> studentIds,
        NpgsqlTransaction transaction
    )
    {
        throw new NotImplementedException();
    }

    private void ResetSyncFlags(List<int> studentIds, NpgsqlTransaction transaction)
    {
        throw new NotImplementedException();
    }
}
