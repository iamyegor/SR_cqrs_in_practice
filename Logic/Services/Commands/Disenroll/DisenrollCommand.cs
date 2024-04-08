using Logic.Services.Commands.Common;

namespace Logic.Services.Commands.Disenroll;

public class DisenrollCommand : ICommand
{
    public int StudentId { get; }
    public int EnrollmentNumber { get; }
    public string Comment { get; }

    public DisenrollCommand(int studentId, int enrollmentNumber, string comment)
    {
        StudentId = studentId;
        EnrollmentNumber = enrollmentNumber;
        Comment = comment;
    }
}
