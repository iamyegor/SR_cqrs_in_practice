using Api.DTOs;
using FluentResults;
using Logic.Application.Commands.Disenroll;
using Logic.Application.Commands.Enroll;
using Logic.Application.Commands.Transfer;
using Logic.Application.Utils;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/students/{studentId}/enrollments")]
public class EnrollmentsController : Controller
{
    private readonly Messages _messages;

    public EnrollmentsController(Messages messages)
    {
        _messages = messages;
    }

    [HttpPost]
    public IActionResult Enroll(int studentId, StudentForEnrollmentDto studentDto)
    {
        EnrollCommand command = (studentId, studentDto).Adapt<EnrollCommand>();
        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }

    [HttpPut("{enrollmentNumber}")]
    public IActionResult Transfer(
        int studentId,
        int enrollmentNumber,
        StudentForTransferDto studentDto
    )
    {
        TransferCommand command = (
            studentId,
            enrollmentNumber,
            studentDto
        ).Adapt<TransferCommand>();

        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }

    [HttpDelete("{enrollmentNumber}")]
    public IActionResult Disenroll(
        int studentId,
        int enrollmentNumber,
        StudentForDisenrollmentDto studentDto
    )
    {
        DisenrollCommand command = (
            studentId,
            enrollmentNumber,
            studentDto
        ).Adapt<DisenrollCommand>();
        
        Result result = _messages.Dispatch(command);

        return FromResult(result);
    }
}
