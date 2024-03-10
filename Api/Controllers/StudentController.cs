using Api.DTOs;
using AutoMapper;
using CSharpFunctionalExtensions;
using Logic.DAL;
using Logic.Students;
using Logic.Students.Commands.EditPersonalInfo;
using Logic.Students.Commands.Register;
using Logic.Students.Commands.Unregister;
using Logic.Students.Queries.GetStudentsList;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : Controller
{
    private readonly Messages _messages;
    private readonly IMapper _mapper;

    public StudentController(Messages messages, IMapper mapper)
    {
        _messages = messages;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetList(string? enrolled, int? number)
    {
        GetStudentsListQuery query = new GetStudentsListQuery(enrolled, number);
        IEnumerable<StudentDto> studentsToReturn = _messages.Dispatch(query);

        return Ok(studentsToReturn);
    }

    [HttpPost]
    public IActionResult Register([FromBody] StudentForCreationDto studentDto)
    {
        RegisterCommand command = _mapper.Map<RegisterCommand>(studentDto);
        Result result = _messages.Dispatch(command);

        return result.IsSuccess ? Ok() : Error(result.Error);
    }

    [HttpDelete("{id}")]
    public IActionResult Unregister(int id)
    {
        Result result = _messages.Dispatch(new UnregisterCommand(id));

        return result.IsSuccess ? Ok() : Error(result.Error);
    }

    [HttpPut("{studentId}")]
    public IActionResult EditPersonalInfo(int studentId, StudentForUpdateDto studentForUpdateDto)
    {
        EditPersonalInfoCommand command = new EditPersonalInfoCommand(
            studentId,
            studentForUpdateDto.Name,
            studentForUpdateDto.Email
        );
        Result result = _messages.Dispatch(command);

        return result.IsSuccess ? Ok() : Error(result.Error);
    }
}
