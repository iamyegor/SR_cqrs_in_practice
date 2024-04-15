using Dapper;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students.Models;
using DbSynchronization.Utils;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class QueryDbStudentRepository
{
    private readonly SqlGenerator _sqlGenerator;

    public QueryDbStudentRepository(SqlGenerator sqlGenerator)
    {
        _sqlGenerator = sqlGenerator;
    }

    public void Save(List<StudentInQueryDb> studentsToSave)
    {
        string query = _sqlGenerator.InsertOrUpdate<StudentInQueryDb>("students", "student_id");

        using var connection = new NpgsqlConnection(QueryDbConnectionString.Value);
        connection.Execute(query, studentsToSave);
    }
}
