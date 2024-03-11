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
        _connectionString = configuration.GetConnectionString("myString")!;
    }

    public IEnumerable<StudentDto> Handle(GetStudentsListQuery query)
    {
        string sql =
            @"
            SELECT s.*, e.grade, c.name as course_name, c.credits  
            FROM students as s  
            LEFT JOIN (  
                SELECT e.student_id, COUNT(*) as number_of_enrollments  
                FROM enrollments as e  
                GROUP BY e.student_id  
            ) as t ON s.id = t.student_id  
            LEFT JOIN enrollments as e ON e.student_id = s.id  
            LEFT JOIN courses as c ON e.course_id = c.id  
            WHERE (c.name = @Course OR @Course IS NULL) 
                AND (COALESCE(t.number_of_enrollments, 0) = @Number OR @Number IS NULL)
            ORDER BY s.id;";

        List<StudentDto> studentsToReturn = [];

        using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
        {
            var parameters = new { Course = query.EnrolledIn, Number = query.NumberOfCourses };
            List<StudentInDb> retrievedStudents = connection
                .Query<StudentInDb>(sql, parameters)
                .ToList();

            List<int> uniqueIds = retrievedStudents.Select(s => s.Id).Distinct().ToList();

            foreach (var id in uniqueIds)
            {
                List<StudentInDb> data = retrievedStudents.Where(s => s.Id == id).ToList();

                var dto = new StudentDto
                {
                    Id = data[0].Id,
                    Name = data[0].Name,
                    Email = data[0].Email,
                    Course1 = data[0].CourseName,
                    Course1Credits = data[0].Credits,
                    Course1Grade = data[0].Grade.ToString()
                };

                if (data.Count > 1)
                {
                    dto.Course2 = data[1].CourseName;
                    dto.Course2Credits = data[1].Credits;
                    dto.Course2Grade = data[1].Grade.ToString();
                }

                studentsToReturn.Add(dto);
            }
        }

        return studentsToReturn;
    }
}

internal class StudentInDb
{
    public readonly int Id;
    public readonly string Name;
    public readonly string Email;
    public readonly Grade? Grade;
    public readonly string CourseName;
    public readonly int? Credits;

    public StudentInDb(
        int id,
        string name,
        string email,
        Grade? grade,
        string courseName,
        int? credits
    )
    {
        Id = id;
        Name = name;
        Email = email;
        Grade = grade;
        CourseName = courseName;
        Credits = credits;
    }
}
