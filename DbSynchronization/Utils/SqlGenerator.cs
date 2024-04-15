using DbSynchronization.Extensions;

namespace DbSynchronization.Utils;

public class SqlGenerator
{
    public string InsertOrUpdate<T>(string tableName, string tablePrimaryKey)
    {
        List<string> propertyNames = typeof(T).GetProperties().Select(p => p.Name).ToList();

        string columnNames = string.Join(", ", propertyNames.Select(p => p.ToSnakeCase()));
        string valueNames = string.Join(", ", propertyNames.Select(p => "@" + p));
        string onConflictUpdate = string.Join(", ", columnNames.Select(c => $"{c} = EXCLUDED.{c}"));

        string query =
            @$"
            insert into {tableName} ({columnNames}) values ({valueNames})
            on conflict ({tablePrimaryKey}) do update set {onConflictUpdate}";

        return query;
    }
}
