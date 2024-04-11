using Logic.Application.Queries.Common;

namespace Logic.Application.Queries.GetStudentsList;

public record GetStudentsListQuery(int? NumberOfEnrollments)
    : IQuery<IReadOnlyList<StudentInDb>>;
