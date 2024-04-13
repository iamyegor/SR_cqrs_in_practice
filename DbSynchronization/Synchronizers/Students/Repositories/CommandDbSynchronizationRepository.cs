using System.Data;
using Dapper;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbSynchronizationRepository
{
    public int GetRowVersionFor(string rowName, NpgsqlTransaction transaction)
    {
        string query = "select row_version from sync where name = @rowName";

        NpgsqlConnection connection = transaction.Connection!;
        return connection.QuerySingle<int>(query, new { rowName }, transaction: transaction);
    }

    public void ResetSyncFlagFor(string rowName, int rowVersion, NpgsqlTransaction transaction)
    {
        string query =
            @"
            update sync 
            set is_sync_required = false
            where name = @rowName and row_version = @rowVersion";

        NpgsqlConnection connection = transaction.Connection!;
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
