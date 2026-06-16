using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Models;
public class AuthController : Controller
{
    private readonly MVCAuthService _service;
    public AuthController(MVCAuthService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _service.Login(model);
        if (user == null)
        {
            model.ErrorMessage = "Invalid email or password.";
            return View(model);
        }
        HttpContext.Session.Clear();
        HttpContext.Session.SetString("Role", user.Role);
        HttpContext.Session.SetString("UserName", user.FirstName);
        return RedirectToAction("Index", "Supplier");
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        var model = new RegisterViewModel
        {
            Countries = await _service.GetCountries()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        foreach (var key in ModelState.Keys)
        {
            var state = ModelState[key];
            foreach (var error in state.Errors)
            {
                Console.WriteLine($"Field: {key} Error: {error.ErrorMessage}");
            }
        }

        if (!ModelState.IsValid)
        {
            model.Countries = await _service.GetCountries();
            model.States = model.CountryId > 0 ? await _service.GetStates(model.CountryId) : new();
            model.Cities = model.StateId > 0 ? await _service.GetCities(model.StateId) : new();
            return View(model);
        }
        var (success, errormsg) = await _service.Register(model);
        if (!success)
        {
            model.ErrorMessage = errormsg;
            model.Countries = await _service.GetCountries();
            model.States = await _service.GetStates(model.CountryId);
            model.Cities = await _service.GetCities(model.StateId);
            return View(model);
        }
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> GetStates(int countryId)
    {
        var states = await _service.GetStates(countryId);
        return Json(states);
    }

    [HttpGet]
    public async Task<IActionResult> GetCities(int stateId)
    {
        var cities = await _service.GetCities(stateId);
        return Json(cities);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}