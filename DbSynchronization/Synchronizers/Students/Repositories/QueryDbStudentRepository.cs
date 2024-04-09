using Dapper;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students.Models;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class QueryDbStudentRepository
{
    public void Save(List<Student> studentsToSave)
    {
        string query =
            @"
            insert into students (
                student_id, 
                name, 
                email, 
                number_of_enrollments, 
                first_course_name, 
                first_course_credits, 
                first_course_grade, 
                second_course_name, 
                second_course_credits, 
                second_course_grade
            ) 
            values (
                @Id,
                @Name,
                @Email,
                @NumberOfEnrollments,
                @FirstCourseName,
                @FirstCourseGrade,
                @FirstCourseCredits,
                @SecondCourseName,
                @SecondCourseGrade,
                @SecondCourseCredits
            )
            on conflict (student_id)
            do update set 
                student_id = excluded.student_id,
                name = excluded.name,
                email = excluded.email,
                number_of_enrollments = excluded.number_of_enrollments,
                first_course_name = excluded.first_course_name,
                first_course_credits = excluded.first_course_credits,
                first_course_grade = excluded.first_course_grade,
                second_course_name = excluded.second_course_name,
                second_course_credits = excluded.second_course_credits,
                second_course_grade = excluded.second_course_grade";

        using var connection = new NpgsqlConnection(QueryDbConnectionString.Value);
        connection.Execute(query, studentsToSave);
    }
}