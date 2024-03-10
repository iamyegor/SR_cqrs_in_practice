using AutoMapper;
using CSharpFunctionalExtensions;
using DTOs;
using Logic.Students;
using Logic.Students.Commands.Disenroll;
using Logic.Students.Commands.Enroll;
using Logic.Students.Commands.Transfer;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students/{studentId}/enrollments")]
public class EnrollmentsController : Controller
{
    private readonly IMapper _mapper;
    private readonly Messages _messages;

    public EnrollmentsController(IMapper mapper, Messages messages)
    {
        _mapper = mapper;
        _messages = messages;
    }

    [HttpPost]
    public IActionResult Enroll(int studentId, StudentForEnrollmentDto studentForEnrollmentDto)
    {
        EnrollCommand command = _mapper.Map<EnrollCommand>(studentForEnrollmentDto);
        command.StudentId = studentId;

        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }

    [HttpPut("{enrollmentNumber}")]
    public IActionResult Transfer(
        int studentId,
        int enrollmentNumber,
        StudentForTransferDto studentForTransferDto
    )
    {
        TransferCommand command = _mapper.Map<TransferCommand>(studentForTransferDto);
        command.StudentId = studentId;
        command.EnrollmentNumber = enrollmentNumber;

        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }

    [HttpDelete("{enrollmentNumber}")]
    public IActionResult Disenroll(
        int studentId,
        int enrollmentNumber,
        StudentForDisenrollmentDto studentForDisenrollmentDto
    )
    {
        DisenrollCommand command = new DisenrollCommand(
            studentId,
            enrollmentNumber,
            studentForDisenrollmentDto.Comment
        );
        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }
}
