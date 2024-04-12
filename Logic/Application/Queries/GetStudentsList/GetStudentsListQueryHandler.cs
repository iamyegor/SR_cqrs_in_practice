using Dapper;
using Logic.Application.Queries.Common;
using Npgsql;

namespace Logic.Application.Queries.GetStudentsList;

public class GetStudentsListQueryHandler
    : IQueryHandler<GetStudentsListQuery, IReadOnlyList<StudentInDb>>
{
    public IReadOnlyList<StudentInDb> Handle(GetStudentsListQuery query)
    {
        string sql =
            @"
            select *
            from students 
            where (first_course_name = @Course or second_course_name = @Course or @Course is null)
            and (number_of_enrollments = @Number or @Number is null)";

        using NpgsqlConnection connection = new NpgsqlConnection(QueryDbConnectionString.Value);
        var param = new { Course = query.EnrolledCourseName, Number = query.NumberOfEnrollments };
        return connection.Query<StudentInDb>(sql, param).ToList();
    }
}
