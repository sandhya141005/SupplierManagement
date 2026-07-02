using Microsoft.AspNetCore.Mvc;

namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{

    [HttpGet("null")]
    public IActionResult ThrowNull()
    {
        string? s = null;
        return Ok(s!.Length);
    }

    [HttpGet("divide")]
    public IActionResult ThrowDivide()
    {
        int x = 0;
        return Ok(10 / x);
    }

    [HttpGet("format")]
    public IActionResult ThrowFormat()
    {
        var date = DateTime.ParseExact("not-a-date", "dd-MMM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture);
        return Ok(date);
    }
    [HttpGet("custom")]
    public IActionResult ThrowCustom()
    {
        throw new Exception("LOGGING FILTER TEST — custom exception from TestController");
    }
}