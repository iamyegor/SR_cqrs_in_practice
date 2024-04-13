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
        string query =
            @"
            select s.*, e.id as enrollment_id, e.*, c.*
            from students s
            left join enrollments e on s.id = e.student_id and is_deleted = false
            left join courses c on e.course_id = c.id
            where s.id = any(@studentIds)";

        var studentDictionary = new Dictionary<int, StudentInCommandDb>();

        NpgsqlConnection connection = transaction.Connection!;
        IEnumerable<StudentInCommandDb> students = connection
            .Query<StudentInCommandDb, EnrollmentInCommandDb, StudentInCommandDb>(
                query,
                (student, enrollment) =>
                {
                    if (!studentDictionary.TryGetValue(student.Id, out var currentStudent))
                    {
                        currentStudent = student;
                        studentDictionary.Add(currentStudent.Id, currentStudent);
                    }

                    currentStudent.Enrollments.Add(enrollment);

                    return currentStudent;
                },
                param: new { studentIds },
                splitOn: "enrollment_id",
                transaction: transaction
            )
            .Distinct();

        return students.ToList();
    }

    private void ResetSyncFlags(List<int> studentIds, NpgsqlTransaction transaction)
    {
        string query = "update Students set is_sync_needed = false where id = any(@studentIds)";

        NpgsqlConnection connection = transaction.Connection!;
        connection.Execute(query, new { studentIds }, transaction: transaction);
    }
}
