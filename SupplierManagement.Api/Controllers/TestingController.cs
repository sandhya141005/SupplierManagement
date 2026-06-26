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
        return Ok(10 / x); // DivideByZeroException
    }

    // Test 3 — FormatException
    [HttpGet("format")]
    public IActionResult ThrowFormat()
    {
        var date = DateTime.ParseExact("not-a-date", "dd-MMM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture); // FormatException
        return Ok(date);
    }

    // Test 4 — Custom exception message
    [HttpGet("custom")]
    public IActionResult ThrowCustom()
    {
        throw new Exception("LOGGING FILTER TEST — custom exception from TestController");
    }
}