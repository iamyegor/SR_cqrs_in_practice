using DbSynchronization.Models;
using Npgsql;

namespace DbSynchronization.Repositories;

public class QueryDbOutboxRepository
{
    public QueryDbOutboxRepository(NpgsqlTransaction transaction, NpgsqlConnection connection)
    {
        throw new NotImplementedException();
    }

    public void Save(List<StudentInQueryDb> studentsToSave)
    {
        throw new NotImplementedException();
    }
}