using Logic.Services.Commands.Common;

namespace Logic.Services.Commands.Register;

public class RegisterCommand : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }

    public string? Course1 { get; set; }
    public string? Course1Grade { get; set; }

    public string? Course2 { get; set; }
    public string? Course2Grade { get; set; }
}
