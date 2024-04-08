using DTOs;
using Logic.Services.Queries.Common;

namespace Logic.Services.Queries.GetStudentsList;

public class GetStudentsListQuery : IQuery<IEnumerable<StudentDto>>
{
    public string? EnrolledIn { get; }
    public int? NumberOfCourses { get; }

    public GetStudentsListQuery(string? enrolledIn, int? numberOfCourses)
    {
        EnrolledIn = enrolledIn;
        NumberOfCourses = numberOfCourses;
    }
}
