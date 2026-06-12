using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using AutoMapper;
namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;
    public AuthController(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    [HttpPost("register")]
    public IActionResult Register(RegisterDTO dto)
    {
        try
        {
            User user = _mapper.Map<User>(dto);
            _service.Register(user);
            return Ok("User Registered Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        /*User user = new User{
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            Password = dto.Password,
            ContactNo = dto.ContactNo,
            Role = dto.IsAdmin?"Admin":"User",
            CountryId = dto.CountryId,
            StateId = dto.StateId,
            CityId = dto.CityId
        };
        _service.Register(user);
        return Ok(
            "User Registered Successfully");
            */

    }
    [HttpPost("login")]
    public IActionResult Login(LoginDTO dto)
    {
        var user = _service.Login(dto.Email, dto.Password);
        if (user == null)
        {
            return Unauthorized("Invalid Credentials");
        }
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(user);
    }
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = new List<UserDTO>
        {
            new UserDTO { FirstName = "Test", Email = "test@gmail.com" }
        };

        return Ok(users);
    }
    
}




