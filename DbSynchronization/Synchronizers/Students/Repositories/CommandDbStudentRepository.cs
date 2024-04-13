using Dapper;
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
        string studentsQuery = "select id from students where is_sync_needed = true for update;";

        NpgsqlConnection connection = transaction.Connection!;
        List<int> studentIds = connection
            .Query<int>(studentsQuery, transaction: transaction)
            .ToList();

        if (studentIds.Count == 0)
        {
            return ([], []);
        }

        string enrollmentsQuery =
            "select id from enrollments where student_id = any(@Ids) for update;";

        List<int> enrollmentIds = connection
            .Query<int>(enrollmentsQuery, new { Ids = studentIds }, transaction: transaction)
            .ToList();

        return (studentIds, enrollmentIds);
    }

    private void DeleteSoftDeletedEnrollments(
        List<int> enrollmentIds,
        NpgsqlTransaction transaction
    )
    {
        string query =
            "delete from enrollments where is_deleted = true and id = any(@enrollmentIds);";

        NpgsqlConnection connection = transaction.Connection!;
        connection.Execute(query, new { enrollmentIds }, transaction: transaction);
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
        string query = "update Students set is_sync_needed = false where id = any(@studentIds)";
        
        NpgsqlConnection connection = transaction.Connection!;
        connection.Execute(query, new { studentIds }, transaction: transaction);
    }
}
