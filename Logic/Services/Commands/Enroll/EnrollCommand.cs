using Logic.Services.Commands.Common;

namespace Logic.Services.Commands.Enroll;

public class EnrollCommand : ICommand
{
    public int StudentId { get; set; }
    public string Course { get; set; }
    public string Grade { get; set; }
}
