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
        List<Student> studentsToReturn = [];

        string query =
            @$"
            select id
            into temp {TempTable}
            from students
            where is_sync_needed = true
            for update;
            
            select 
                s.id as Id,
                s.name as Name,
                s.email as Email,
                t.number_of_enrollments as NumberOfEnrollments,
                e.grade as Grade,
                c.name as CourseName,
                c.credits as CourseCredits
            from students s
            left join (
                select student_id, count(*) as number_of_enrollments
                from enrollments
                group by student_id
            ) t on s.id = t.student_id
            left join enrollments e on s.id = e.student_id 
            left join courses c on c.id = e.course_id
            where s.is_sync_needed = true";

        List<StudentInCommandDb> retrievedStudents = connection
            .Query<StudentInCommandDb>(query, transaction: transaction)
            .ToList();

        IEnumerable<int> uniqueStudentIds = retrievedStudents.Select(s => s.Id).Distinct();

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
