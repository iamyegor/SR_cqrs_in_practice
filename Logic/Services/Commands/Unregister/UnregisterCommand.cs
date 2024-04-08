using Logic.Services.Commands.Common;

namespace Logic.Services.Commands.Unregister;

public class UnregisterCommand : ICommand
{
    public int Id { get; }

    public UnregisterCommand(int id)
    {
        Id = id;
    }
}
