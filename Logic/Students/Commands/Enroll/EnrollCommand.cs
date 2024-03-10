using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Enroll;

public class EnrollCommand : ICommand
{
    public int StudentId { get; set; }
    public string Course { get; }
    public string Grade { get; }

    public EnrollCommand(int studentId, string course, string grade)
    {
        StudentId = studentId;
        Course = course;
        Grade = grade;
    }
}
