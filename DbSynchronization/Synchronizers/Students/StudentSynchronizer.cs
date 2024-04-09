using Dapper;
using DbSynchronization.ConnectionStrings;
using DbSynchronization.Synchronizers.Students.Models;
using DbSynchronization.Synchronizers.Students.Repositories;
using Npgsql;

namespace DbSynchronization.Synchronizers.Students;

public class StudentSynchronizer
{
    private readonly CommandDbStudentRepository _studentRepository;
    private readonly CommandDbSynchronizationRepository _syncRepository;
    private readonly CommandDbOutboxRepository _outboxRepository;

    public StudentSynchronizer(
        CommandDbStudentRepository studentRepository,
        CommandDbSynchronizationRepository syncRepository,
        CommandDbOutboxRepository outboxRepository
    )
    {
        _studentRepository = studentRepository;
        _syncRepository = syncRepository;
        _outboxRepository = outboxRepository;
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

        try
        {
            List<Student> studentsFromCommandDb = GetUpdatedStudents(
                connection,
                transaction
            );

            _outboxRepository.Save(studentsFromCommandDb, "Student", connection, transaction);

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
    }

    private bool IsSyncNeeded()
    {
        string query = "select is_sync_required from sync where name = 'Student'";
        using var connection = new NpgsqlConnection(CommandDbConnectionString.Value);
        return connection.QuerySingle<bool>(query);
    }

    private List<Student> GetUpdatedStudents(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction
    )
    {
        int syncRowVersion = _syncRepository.GetRowVersionFor("Student", connection, transaction);

        List<Student> students = _studentRepository.GetAllThatNeedSync(
            connection,
            transaction
        );

        _studentRepository.SetSyncFlagsFalse(connection, transaction);
        _syncRepository.SetSyncFlagFalseFor("Student", syncRowVersion, connection, transaction);

        return students;
    }
}
