using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Enroll;

public class EnrollCommand : ICommand
{
    public int StudentId { get; set; }
    public string Course { get; set; }
    public string Grade { get; set; }
}
