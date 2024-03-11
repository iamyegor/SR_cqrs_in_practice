using AutoMapper;
using Dapper;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Models;
using DbSynchronization.Repositories;
using Npgsql;

namespace DbSynchronization.Synchronizers;

public class StudentSynchronizer
{
    private readonly IMapper _mapper;

    public StudentSynchronizer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void Sync()
    {
        if (!IsSyncNeeded())
        {
            return;
        }

        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        List<StudentInCommandDb> studentsFromCommandDb = GetUpdatedStudents(
            connection,
            transaction
        );

        List<StudentInQueryDb> studentsToSave = _mapper.Map<List<StudentInQueryDb>>(
            studentsFromCommandDb
        );

        var outboxRepository = new QueryDbOutboxRepository(transaction, connection);
        outboxRepository.Save(studentsToSave);
    }

    private bool IsSyncNeeded()
    {
        string query = "select is_sync_required from sync where name = 'Student'";
        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);
        return connection.QuerySingle(query);
    }

    private List<StudentInCommandDb> GetUpdatedStudents(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        var studentRepository = new CommandDbStudentRepository(connection, transaction);
        var syncRepository = new CommandDbSynchronizationRepository(connection, transaction);

        int syncRowVersion = syncRepository.GetRowVersionFor("Student");

        List<StudentInCommandDb> students = studentRepository.GetAllThatNeedSync();

        studentRepository.SetSyncFlagsFalse();
        syncRepository.SetSyncFlagFalseFor("Student", syncRowVersion);

        return students;
    }
}
