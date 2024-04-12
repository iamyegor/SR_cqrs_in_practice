using Api.DTOs;
using FluentResults;
using Logic.Application.Commands.EditPersonalInfo;
using Logic.Application.Commands.Register;
using Logic.Application.Commands.Unregister;
using Logic.Application.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly Messages _messages;

    public StudentController(Messages messages)
    {
        _messages = messages;
    }

    [HttpGet]
    public IActionResult GetList(string? enrolled, int? number)
    {
        // IReadOnlyList<Student> studentsFromDb = _studentRepository.GetList(enrolled, number);
        //
        // return Ok(studentsFromDb.Adapt<IEnumerable<StudentDto>>())
        return Ok();
    }

    [HttpPost]
    public IActionResult Register(StudentForRegistrationDto studentDto)
    {
        RegisterCommand command = studentDto.Adapt<RegisterCommand>();
        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Unregister(int id)
    {
        Result result = _messages.Dispatch(new UnregisterCommand(id));

        return FromResult(result);
    }

    [HttpPut("{studentId}")]
    public IActionResult EditPersonalInfo(
        int studentId,
        StudentForEditPersonalInfoDto studentForEditPersonalInfoDto
    )
    {
        EditPersonalInfoCommand command = (
            studentId,
            studentForEditPersonalInfoDto
        ).Adapt<EditPersonalInfoCommand>();

        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }
}
