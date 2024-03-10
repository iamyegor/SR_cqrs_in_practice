using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Students.Commands.Common;

namespace Logic.Students.Commands.EditPersonalInfo;

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
