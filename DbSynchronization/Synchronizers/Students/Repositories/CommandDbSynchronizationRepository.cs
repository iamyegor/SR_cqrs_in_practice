using System.Data;
using Dapper;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbSynchronizationRepository
{
    public int GetRowVersionFor(
        string rowName,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string query = "select row_version from sync where name = @rowName";
        return connection.QuerySingle<int>(query, new { rowName }, transaction: transaction);
    }

    public void SetSyncFlagFalseFor(
        string rowName,
        int rowVersion,
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        string query =
            @"
            update sync 
            set is_sync_required = false
            where name = @rowName and row_version = @rowVersion";

        int rowsAffected = connection.Execute(
            query,
            new { rowName, rowVersion },
            transaction: transaction
        );

        if (rowsAffected == 0)
        {
            throw new DBConcurrencyException($"Conflict in {rowName} in Synchronization table");
        }
    }
}
