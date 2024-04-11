using Logic.Application.Commands.Common;

namespace Logic.Application.Commands.EditPersonalInfo;

public record EditPersonalInfoCommand(int StudentId, string Name, string Email) : ICommand;
