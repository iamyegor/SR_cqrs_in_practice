using Logic.Application.Commands.Common;

namespace Logic.Application.Commands.Unregister;

public record UnregisterCommand(int StudentId) : ICommand;
