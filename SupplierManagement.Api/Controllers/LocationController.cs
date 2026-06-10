using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Data.Context;
namespace SupplierManagement.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly AppDbContext _context;

    public LocationController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("countries")]
    public IActionResult GetCountries()
    {
        var countries = _context.Countries.Select(c => new {Id=c.CountryId, Name=c.CountryName}).ToList();
        return Ok(countries);
    }

    [HttpGet("states/{countryId}")]
    public IActionResult GetStates(int countryId)
    {
        var states = _context.States
            .Where(s => s.CountryId == countryId)
            .Select(s => new { Id = s.StateId, Name = s.StateName })
            .ToList();
        return Ok(states);
    }

    [HttpGet("cities/{stateId}")]
    public IActionResult GetCities(int stateId)
    {
        var cities = _context.Cities
            .Where(c => c.StateId == stateId)
            .Select(c => new {  Id = c.CityId, Name = c.CityName  })
            .ToList();
        return Ok(cities);
    }
}