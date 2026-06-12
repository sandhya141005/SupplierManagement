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
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {


        await _service.Delete(id);
        return RedirectToAction("Index");

    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _service.GetById(id);
        if (supplier == null)
            return NotFound();
        return View(supplier);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, SupplierViewModel supplier)
    {
        var success = await _service.Edit(id, supplier);
        if (!success)
        {
            ModelState.AddModelError("", "Failed to update supplier");
            return View(supplier);

        }
        return RedirectToAction("Index");

    }
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var model = new SupplierViewModel { Countries = await _service.GetCountries() };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SupplierViewModel supplier)
    {
        /*ModelState.Remove("Countries");
        ModelState.Remove("States");
        ModelState.Remove("Cities");*/
        ModelState.Remove("ErrorMessage");
        ModelState.Remove("CreatedDate");
        ModelState.Remove("Country");
        ModelState.Remove("State");
        ModelState.Remove("City");

        if (!ModelState.IsValid)
        {
            supplier.Countries = await _service.GetCountries();
            supplier.States=supplier.CountryId>0?await _service.GetStates(supplier.CountryId):new ();
            supplier.Cities=supplier.StateId>0?await _service.GetCities(supplier.StateId): new();
            return View(supplier);
        }
        var (success, error) = await _service.Add(supplier);
        if (!success)
        {
            ModelState.AddModelError("", error);
            supplier.Countries = await _service.GetCountries();
            supplier.States = supplier.CountryId > 0 ? await _service.GetStates(supplier.CountryId) : new();
            supplier.Cities = supplier.StateId > 0 ? await _service.GetCities(supplier.StateId) : new();
            return View(supplier);
        }
        return RedirectToAction("Index");

    }


}