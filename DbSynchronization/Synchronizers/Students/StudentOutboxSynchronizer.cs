using DbSynchronization.Synchronizers.Students.Models;
using DbSynchronization.Synchronizers.Students.Repositories;

namespace DbSynchronization.Synchronizers.Students;

public class StudentOutboxSynchronizer
{
    private readonly CommandDbOutboxRepository _commandDbOutboxRepository;
    private readonly QueryDbStudentRepository _queryDbStudentRepository;

    public StudentOutboxSynchronizer(
        CommandDbOutboxRepository commandDbOutboxRepository,
        QueryDbStudentRepository queryDbStudentRepository
    )
    {
        _commandDbOutboxRepository = commandDbOutboxRepository;
        _queryDbStudentRepository = queryDbStudentRepository;
    }

    public void Sync()
    {
        (List<int> ids, List<StudentInQueryDb> studentsFromOutbox) =
            _commandDbOutboxRepository.Get<StudentInQueryDb>("Student");

        if (ids.Count == 0)
        {
            return;
        }

        _queryDbStudentRepository.Save(studentsFromOutbox);

        _commandDbOutboxRepository.Remove(ids);
    }
}
