using Logic.Application.Queries.Common;

namespace Logic.Application.Queries.GetStudentsList;

public record GetStudentsListQuery(string? EnrolledCourseName, int? NumberOfEnrollments)
    : IQuery<IReadOnlyList<StudentInDb>>;
