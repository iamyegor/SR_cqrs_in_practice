using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.Unregister;

public class UnregisterCommand : ICommand
{
    public int Id { get; }

    public UnregisterCommand(int id)
    {
        Id = id;
    }
}
