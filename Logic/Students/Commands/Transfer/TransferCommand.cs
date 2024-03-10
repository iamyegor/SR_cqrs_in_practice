using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Transfer;

public class TransferCommand : ICommand
{
    public int StudentId { get; set; }
    public int EnrollmentNumber { get; set; }
    public string Course { get; set; }
    public string Grade { get; set; }
}
