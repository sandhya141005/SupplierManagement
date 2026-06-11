using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Models;
public class SupplierController : Controller
{
    private readonly MVCSupplierService _service;
    public SupplierController(MVCSupplierService service)
    {
        _service = service;
    }
    public async Task<IActionResult> Index()
    {
        var suppliers = await _service.GetAll();
        ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";
        return View(suppliers);
    }
}