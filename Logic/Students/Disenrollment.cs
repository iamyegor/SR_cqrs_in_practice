using Logic.Students.Common;

namespace Logic.Students;

public class Disenrollment : Entity
{
    public Student? Student { get; }
    public Course? Course { get; }
    public DateTime DateTime { get; }
    public string Comment { get; }

    private Disenrollment()
        : base(0) { }

    public Disenrollment(Student? student, Course? course, string comment)
        : base(0)
    {
        Student = student;
        Course = course;
        Comment = comment;
        DateTime = DateTime.UtcNow;
    }
}
