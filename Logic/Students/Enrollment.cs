using Logic.Students.Common;

namespace Logic.Students;

public class Enrollment : Entity
{
    public Student? Student { get; private set; }
    public Course Course { get; private set; }
    public Grade Grade { get; private set; }

    private Enrollment()
        : base(0) { }

    public Enrollment(Student? student, Course course, Grade grade)
        : base(0)
    {
        Student = student;
        Course = course;
        Grade = grade;
    }

    public void Update(Course course, Grade grade)
    {
        Course = course;
        Grade = grade;
    }
}
