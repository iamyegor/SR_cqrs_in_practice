using Logic.Application.Commands.Common;

namespace Logic.Application.Commands.Transfer;

public record TransferCommand(int StudentId, int EnrollmentNumber, string Course, string Grade)
    : ICommand;
