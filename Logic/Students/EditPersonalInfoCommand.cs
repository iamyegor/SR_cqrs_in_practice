using CSharpFunctionalExtensions;
using Logic.DAL;

namespace Logic.Students;

public class EditPersonalInfoCommand : ICommand
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }

    public EditPersonalInfoCommand(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}

public interface ICommand;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public Result Handle(TCommand command);
}

public class EditPersonalInfoCommandHandler : ICommandHandler<EditPersonalInfoCommand>
{
    private readonly StudentRepository _studentRepository;

    public EditPersonalInfoCommandHandler(StudentRepository studentRepository)
    {
        ArgumentNullException.ThrowIfNull(studentRepository);

        _studentRepository = studentRepository;
    }

    public Result Handle(EditPersonalInfoCommand command)
    {
        Student? student = _studentRepository.GetById(command.Id);
        if (student == null)
        {
            return Result.Failure($"No student found for Id {command.Id}");
        }

        student.Name = command.Name;
        student.Email = command.Email;

        _studentRepository.Save(student);

        return Result.Success();
    }
}
