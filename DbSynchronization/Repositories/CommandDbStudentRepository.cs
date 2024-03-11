using System;
using System.Collections.Generic;
using Dapper;
using DbSynchronization.Models;
using Npgsql;

namespace DbSynchronization.Repositories;

public class CommandDbStudentRepository
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction _transaction;

    public CommandDbStudentRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public List<Student> GetAllThatNeedSync()
    {
        string query =
            @"
select 
    s.id as StudentId,
    s.name as Name,
    s.email as Email,
    t.number_of_enrollments as NumberOfEnrollments,
    e.grade as Grade,
    c.name as Name,
    c.credits as Credits
from students s
left join (
    select student_id, count(*) as number_of_enrollments
    from enrollments
    group by student_id
) t on s.id = t.student_id
left join enrollments e on s.id = e.student_id
left join courses c on c.id = e.course_id
where s.is_sync_needed = true";

        _connection.Query<Student>(query);
    }

    public void SetSyncFlagsFalse()
    {
        throw new NotImplementedException();
    }
}
