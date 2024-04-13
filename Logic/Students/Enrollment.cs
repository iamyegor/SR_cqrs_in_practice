namespace Logic.Students;

public class Enrollment : Entity
{
    public Student? Student { get; private set; }
    public Course Course { get; private set; }
    public Grade Grade { get; private set; }
    public bool IsDeleted { get; set; }

    private Enrollment() { }

    public Enrollment(Student? student, Course course, Grade grade)
        : this()
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
