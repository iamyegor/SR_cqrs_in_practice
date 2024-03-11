using Dapper;
using Npgsql;

namespace DbSynchronization.Repositories;

public class CommandDbSynchronizationRepository
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction _transaction;

    public CommandDbSynchronizationRepository(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        _connection = connection;
        _transaction = transaction;
    }

    public int GetRowVersionFor(string rowName)
    {
        string query = "select row_version from sync where name = @rowName";
        return _connection.QuerySingle<int>(query, new { rowName }, transaction: _transaction);
    }

    public void SetSyncFlagFalseFor(string rowName, int rowVersion)
    {
        string query =
            @"
            update sync 
            set is_sync_required = false
            where name = @rowName and row_version = @rowVersion";

        _connection.Execute(query, new { rowName, rowVersion }, transaction: _transaction);
    }
}
