using Dapper;
using DTOs;
using Logic.Students.Queries.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Logic.Students.Queries.GetStudentsList;

public class GetStudentsListQueryHandler
    : IQueryHandler<GetStudentsListQuery, IEnumerable<StudentDto>>
{
    private readonly string _connectionString;

    public GetStudentsListQueryHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("queries")!;
    }

    public IEnumerable<StudentDto> Handle(GetStudentsListQuery query)
    {
        string sql =
            @"SELECT 
                student_id AS Id, 
                name AS Name, 
                email AS Email, 
                first_course_name AS Course1, 
                first_course_credits AS Course1Credits, 
                first_course_grade AS Course1Grade,  
                second_course_name AS Course2, 
                second_course_credits AS Course2Credits, 
                second_course_grade AS Course2Grade 
            FROM 
                students s
            WHERE 
                (s.first_course_name = @Course 
                OR s.second_course_name = @Course 
                OR @Course IS NULL)
            AND 
                (s.number_of_enrollments = @Number 
                OR @Number IS NULL)
            ORDER BY 
                s.student_id";

        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var parameters = new { Course = query.EnrolledIn, Number = query.NumberOfCourses };
            List<StudentDto> retrievedStudents = connection
                .Query<StudentDto>(sql, parameters)
                .ToList();

            return retrievedStudents;
        }
    }
}
