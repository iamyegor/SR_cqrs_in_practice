using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.EditPersonalInfo;

public class EditPersonalInfoCommand : ICommand
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }

    public EditPersonalInfoCommand(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}