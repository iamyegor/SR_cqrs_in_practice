using FluentResults;
using Logic.Application.Commands.Common;
using Logic.DAL.Repositories;
using Logic.Students;

namespace Logic.Application.Commands.EditPersonalInfo;

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
        Student? student = _studentRepository.GetById(command.StudentId);
        if (student == null)
        {
            return Result.Fail($"No student found for Id {command.StudentId}");
        }

        student.Name = command.Name;
        student.Email = command.Email;

        _studentRepository.Save(student);

        return Result.Ok();
    }
}
