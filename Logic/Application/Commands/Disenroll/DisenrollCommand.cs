using Logic.Application.Commands.Common;

namespace Logic.Application.Commands.Disenroll;

public record DisenrollCommand(int StudentId, int EnrollmentNumber, string Comment) : ICommand;
