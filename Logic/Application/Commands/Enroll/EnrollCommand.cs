using Logic.Application.Commands.Common;

namespace Logic.Application.Commands.Enroll;

public record EnrollCommand(int StudentId, string Course, string Grade) : ICommand;
