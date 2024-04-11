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
            where (number_of_enrollments = @NumberOfEnrollments or @NumberOfEnrollments is null)";

        using NpgsqlConnection connection = new NpgsqlConnection(QueryDbConnectionString.Value);
        return connection.Query<StudentInDb>(sql, new { query.NumberOfEnrollments }).ToList();
    }
}
