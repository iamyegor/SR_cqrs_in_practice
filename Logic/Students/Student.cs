namespace Logic.Students;

public class Student : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }

    private readonly IList<Enrollment> _enrollments = new List<Enrollment>();
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
    private List<Enrollment> _removedEnrollments = [];
    public Enrollment? FirstEnrollment => GetEnrollment(0);
    public Enrollment? SecondEnrollment => GetEnrollment(1);

    private readonly IList<Disenrollment> _disenrollments = new List<Disenrollment>();
    public IReadOnlyList<Disenrollment> Disenrollments => _disenrollments.ToList();

    private Student() { }

    public Student(string name, string email)
    {
        Name = name;
        Email = email;
    }

    private Enrollment? GetEnrollment(int index)
    {
        if (_enrollments.Count > index)
        {
            return _enrollments[index];
        }

        return null;
    }

    public void RemoveEnrollment(Enrollment enrollment)
    {
        _enrollments.Remove(enrollment);
        _removedEnrollments.Add(enrollment);
    }

    public IEnumerable<Enrollment> PopRemovedEnrollments()
    {
        IEnumerable<Enrollment> copy = _removedEnrollments.ToList();

        _removedEnrollments.Clear();

        return copy;
    }

    public void AddDisenrollmentComment(Enrollment enrollment, string comment)
    {
        var disenrollment = new Disenrollment(enrollment.Student, enrollment.Course, comment);
        _disenrollments.Add(disenrollment);
    }

    public void Enroll(Course course, Grade grade)
    {
        if (_enrollments.Count >= 2)
        {
            throw new Exception("Cannot have more than 2 enrollments");
        }

        var enrollment = new Enrollment(this, course, grade);
        _enrollments.Add(enrollment);
    }
}
