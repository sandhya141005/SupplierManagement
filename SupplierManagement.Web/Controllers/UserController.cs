using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly MVCUserService _service;

    public UserController(MVCUserService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _service.GetUsers();
        return View(users);
    }
}