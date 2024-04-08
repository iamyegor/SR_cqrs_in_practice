using Logic.Services.Commands.Common;

namespace Logic.Services.Commands.EditPersonalInfo;

public class EditPersonalInfoCommand : ICommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
