using System.Collections.Generic;

namespace DbSynchronization.Models;

public class StudentInCommandDb
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<EnrollmentInCommandDb> Enrollments { get; set; } = null!;
}

public class EnrollmentInCommandDb
{
    public CourseInCommandDb Course { get; set; } = null!;
    public int Grade { get; set; }
}

public class CourseInCommandDb
{
    public string Name { get; set; } = null!;
    public int Credits { get; set; }
}
