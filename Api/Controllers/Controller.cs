using Api.Utils;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class Controller : ControllerBase
{
    protected new IActionResult Ok()
    {
        return base.Ok(Envelope.Ok());
    }

    protected IActionResult Ok<T>(T result)
    {
        return base.Ok(Envelope.Ok(result));
    }

    protected IActionResult Error(string? errorMessage)
    {
        return BadRequest(Envelope.Error(errorMessage));
    }

    protected IActionResult FromResult(Result result)
    {
        return result.IsSuccess ? Ok() : Error(result.Errors.Single().Message);
    }
}
