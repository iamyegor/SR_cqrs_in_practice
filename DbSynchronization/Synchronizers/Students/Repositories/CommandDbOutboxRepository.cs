using Dapper;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students.Models;
using Newtonsoft.Json;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbOutboxRepository
{
    public void Save(IEnumerable<object> objectsToSave, string type, NpgsqlTransaction transaction)
    {
        var jsonListToSave = objectsToSave.Select(obj => new
        {
            Content = JsonConvert.SerializeObject(obj)
        });

        string query =
            @$"
            insert into outbox (content, type)
            values (@Content::jsonb, '{type}')";

        NpgsqlConnection connection = transaction.Connection!;
        connection.Execute(query, jsonListToSave, transaction: transaction);
    }

    public (List<int> ids, List<T> studentsFromOutbox) Get<T>(string type)
    {
        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);

        string query = "select id, content from outbox where type = @type";

        List<OutboxRow> outboxRows = connection.Query<OutboxRow>(query, new { type }).ToList();

        List<T> objectsToReturn = [];
        foreach (var json in outboxRows.Select(r => r.Content))
        {
            T? deserializedObject = JsonConvert.DeserializeObject<T>(json);

            if (deserializedObject == null)
            {
                throw new Exception("Couldn't deserialize outbox entry");
            }

            objectsToReturn.Add(deserializedObject);
        }

        List<int> ids = outboxRows.Select(r => r.Id).ToList();

        return (ids, objectsToReturn);
    }

    public void Remove(List<int> ids)
    {
        string query =
            @"
            delete from outbox
            where id = @id";

        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);
        connection.Execute(query, ids.Select(id => new { id }));
    }
}
