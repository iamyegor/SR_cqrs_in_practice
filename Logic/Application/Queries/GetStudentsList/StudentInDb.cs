namespace Logic.Application.Queries.GetStudentsList;

public class StudentInDb
{
    public long StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string? FirstCourseName { get; set; }
    public string? FirstCourseGrade { get; set; }
    public int? FirstCourseCredits { get; set; }

    public string? SecondCourseName { get; set; }
    public string? SecondCourseGrade { get; set; }
    public int? SecondCourseCredits { get; set; }
}
