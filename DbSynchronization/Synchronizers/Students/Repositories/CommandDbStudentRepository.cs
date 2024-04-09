using Dapper;
using DbSynchronization.Synchronizers.Students.Models;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbStudentRepository
{
    public List<Student> SyncAndResetFlags(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        (List<int> studentIds, List<int> enrollmentIds) = LockStudentsAndEnrollments(
            connection,
            transaction
        );

        List<StudentInJoinQuery> retrievedStudents = GetStudentsInCommandDb(
            connection,
            transaction
        );

        DeleteSoftDeletedEnrollments(enrollmentIds, connection, transaction);
        ResetSyncFlags(studentIds, connection, transaction);

        List<Student> students = MapStudents(retrievedStudents);

        return students;
    }

    private (List<int> studentIds, List<int> enrollmentIds) LockStudentsAndEnrollments(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string studentsQuery = "select id from students where is_sync_needed = true for update;";
        List<int> studentIds = connection
            .Query<int>(studentsQuery, transaction: transaction)
            .ToList();

        if (studentIds.Count == 0)
        {
            return ([], []); // empty lists, not eyes
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
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string query =
            "delete from enrollments where is_deleted = true and id = any(@enrollmentIds);";

        connection.Execute(query, new { enrollmentIds }, transaction: transaction);
    }

    private List<StudentInJoinQuery> GetStudentsInCommandDb(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string query =
            @$"
            SELECT 
                s.id AS Id, 
                s.name AS Name, 
                s.email AS Email,
                (SELECT COUNT(*) FROM enrollments e WHERE e.student_id = s.id and e.is_deleted = false) 
                    AS NumberOfEnrollments,
                e.grade AS Grade, 
                e.is_deleted as IsCourseDeleted,
                c.name AS CourseName, 
                c.credits AS CourseCredits
            FROM students s
            left JOIN enrollments e ON s.id = e.student_id
            left JOIN courses c ON e.course_id = c.id
            WHERE s.is_sync_needed = true;";

        List<StudentInJoinQuery> retrievedStudents = connection
            .Query<StudentInJoinQuery>(query, transaction: transaction)
            .ToList();
        return retrievedStudents;
    }

    private void ResetSyncFlags(
        List<int> studentIds,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string query =
            @"
                update Students
                set is_sync_needed = false
                where id = any(@studentIds)";

        connection.Execute(query, new { studentIds }, transaction: transaction);
    }

    private List<Student> MapStudents(List<StudentInJoinQuery> retrievedStudents)
    {
        return retrievedStudents
            .GroupBy(s => s.Id)
            .Select(g =>
            {
                StudentInJoinQuery firstEntry = g.First();
                var student = new Student
                {
                    Id = firstEntry.Id,
                    Name = firstEntry.Name,
                    Email = firstEntry.Email,
                    NumberOfEnrollments = firstEntry.NumberOfEnrollments,
                };

                if (!firstEntry.IsCourseDeleted)
                {
                    student.FirstCourseName = firstEntry.CourseName;
                    student.FirstCourseCredits = firstEntry.CourseCredits;
                    student.FirstCourseGrade = firstEntry.Grade;
                }

                StudentInJoinQuery? secondEntry = g.ElementAtOrDefault(1);
                if (secondEntry is { IsCourseDeleted: false })
                {
                    student.SecondCourseName = secondEntry.CourseName;
                    student.SecondCourseCredits = secondEntry.CourseCredits;
                    student.SecondCourseGrade = secondEntry.Grade;
                }

                return student;
            })
            .ToList();
    }
}
