using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.EditPersonalInfo;

public class EditPersonalInfoCommand : ICommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
