using Logic.Application.Commands.Common;

namespace Logic.Application.Commands.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string? Course1,
    string? Course1Grade,
    string Course2,
    string? Course2Grade
) : ICommand;
