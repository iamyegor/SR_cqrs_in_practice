using DbSynchronization.Synchronizers.Students.Models;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students.Repositories;

public class CommandDbStudentRepository
{
    public IReadOnlyList<StudentInCommandDb> GetForSyncAndResetFlags(NpgsqlTransaction transaction)
    {
        throw new NotImplementedException();
    }
}
