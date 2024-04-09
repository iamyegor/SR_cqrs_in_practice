using Dapper;
using DbSynchronization.Synchronizers.Students.Models;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbStudentRepository
{
    private const string TempTable = "students_to_sync";

    public List<Student> GetAllThatNeedSync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        LockStudentsAndEnrollments(connection, transaction);

        List<StudentInCommandDb> retrievedStudents = GetStudentsInCommandDb(
            connection,
            transaction
        );

        List<Student> students = MapStudents(retrievedStudents);

        return students;
    }

    private void LockStudentsAndEnrollments(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string studentsQuery = "select id from students where is_sync_needed = true for update;";
        List<int> studentIds = connection
            .Query<int>(studentsQuery, transaction: transaction)
            .ToList();

        if (studentIds.Count != 0)
        {
            string enrollmentsQuery =
                "select id from enrollments where student_id = any(@Ids) for update;";

            connection.Execute(
                enrollmentsQuery,
                new { Ids = studentIds },
                transaction: transaction
            );
        }
    }

    private List<StudentInCommandDb> GetStudentsInCommandDb(
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
                (SELECT COUNT(*) FROM enrollments e WHERE e.student_id = s.id) 
                    AS NumberOfEnrollments,
                e.grade AS Grade, 
                c.name AS CourseName, 
                c.credits AS CourseCredits
            into temp {TempTable}
            FROM students s
            left JOIN enrollments e ON s.id = e.student_id
            left JOIN courses c ON e.course_id = c.id
            WHERE s.is_sync_needed = true;

            select * from {TempTable}";

        List<StudentInCommandDb> retrievedStudents = connection
            .Query<StudentInCommandDb>(query, transaction: transaction)
            .ToList();
        return retrievedStudents;
    }

    private List<Student> MapStudents(List<StudentInCommandDb> retrievedStudents)
    {
        IEnumerable<int> uniqueStudentIds = retrievedStudents.Select(s => s.Id).Distinct();

        List<Student> studentsToReturn = [];
        foreach (var uniqueStudentId in uniqueStudentIds)
        {
            List<StudentInCommandDb> data = retrievedStudents
                .Where(s => s.Id == uniqueStudentId)
                .ToList();

            Student student = new Student()
            {
                Id = data[0].Id,
                Name = data[0].Name,
                Email = data[0].Email,
                NumberOfEnrollments = data[0].NumberOfEnrollments,
                FirstCourseName = data[0].CourseName,
                FirstCourseCredits = data[0].CourseCredits,
                FirstCourseGrade = data[0].Grade,
            };

            // The join operation might include duplicates of a student with different course
            // enrollments due to the one-to-many relationship between students and enrollments.
            // We expect a maximum of 2 enrollments per student. If more than one record is found
            // for a student, it indicates multiple course enrollments.
            if (data.Count > 1)
            {
                student.SecondCourseName = data[1].CourseName;
                student.SecondCourseCredits = data[1].CourseCredits;
                student.SecondCourseGrade = data[1].Grade;
            }

            studentsToReturn.Add(student);
        }

        return studentsToReturn;
    }

    public void SetSyncFlagsFalse(NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        string query =
            @$"
            update Students
            set is_sync_needed = false
            where id in (select id from {TempTable})";

        connection.Execute(query, transaction: transaction);
    }
}
