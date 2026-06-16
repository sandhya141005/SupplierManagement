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
        supplier.SelectedPaymentMethods = supplier.PaymentMethodsAllowed.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

        supplier.Countries = await _service.GetCountries();
        supplier.States = supplier.CountryId > 0 ? await _service.GetStates(supplier.CountryId) : new();
        supplier.Cities = supplier.StateId > 0 ? await _service.GetCities(supplier.StateId) : new();
        return View(supplier);
    }
   
   [HttpPost]
public async Task<IActionResult> Edit(int id, SupplierViewModel supplier)
{
    ModelState.Remove("PaymentMethodsAllowed");
    ModelState.Remove("ErrorMessage");
    ModelState.Remove("CreatedDate");
    ModelState.Remove("Country");
    ModelState.Remove("State");
    ModelState.Remove("City");
    ModelState.Remove("ContactNo");

    supplier.PaymentMethodsAllowed = string.Join(", ", supplier.SelectedPaymentMethods);

    supplier.Products = supplier.Products
        .Where(p => !string.IsNullOrWhiteSpace(p.ProductName))
        .ToList();

    if (!ModelState.IsValid)
    {
        await ReloadDropdowns(supplier);
        return View(supplier);
    }

    if (supplier.Products.Count != supplier.TotalProducts)
    {
        ModelState.AddModelError("", $"Please ensure exactly {supplier.TotalProducts} product(s). You have {supplier.Products.Count}.");
        await ReloadDropdowns(supplier);
        return View(supplier);
    }

    var (success, error) = await _service.Edit(id, supplier);
    if (!success)
    {
        ModelState.AddModelError("", $"Failed to update supplier: {error}");
        await ReloadDropdowns(supplier);
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
        ModelState.Remove("PaymentMethodsAllowed");
        ModelState.Remove("ErrorMessage");
        ModelState.Remove("CreatedDate");
        ModelState.Remove("Country");
        ModelState.Remove("State");
        ModelState.Remove("City");
        ModelState.Remove("ContactNo");

        supplier.PaymentMethodsAllowed = string.Join(", ", supplier.SelectedPaymentMethods);

        // strip empty trailing rows browser may have submitted
        supplier.Products = supplier.Products.Where(p => !string.IsNullOrWhiteSpace(p.ProductName)).ToList();
        if (!ModelState.IsValid)
        {
            await ReloadDropdowns(supplier);
            return View(supplier);
        }
        if (supplier.Products.Count == 0)
        {
            ModelState.AddModelError("", "At least one product is required");
            await ReloadDropdowns(supplier);
            return View(supplier);
        }

        if (supplier.Products.Count != supplier.TotalProducts)
        {
            ModelState.AddModelError("", $"Please add exactly {supplier.TotalProducts} product(s). You added {supplier.Products.Count}.");
            await ReloadDropdowns(supplier);
            return View(supplier);
        }

        foreach (var p in supplier.Products)
        {
            if (string.IsNullOrWhiteSpace(p.Category))
                p.Category = supplier.CatalogType;
            if (p.Price <= 0)
            {
                ModelState.AddModelError("", $"Price for '{p.ProductName}' must be greater than 0.");
            }
            if (p.AvailableStock < 0)
            {
                ModelState.AddModelError("", $"Stock for '{p.ProductName}' cannot be negative.");
            }
        }

        if (!ModelState.IsValid)
        {
            await ReloadDropdowns(supplier);
            return View(supplier);
        }

        var (success, error) = await _service.Add(supplier);
        if (!success)
        {
            ModelState.AddModelError("", error);
            await ReloadDropdowns(supplier);
            return View(supplier);
        }

        return RedirectToAction("Index");
    }

    private async Task ReloadDropdowns(SupplierViewModel supplier)
    {
        supplier.Countries = await _service.GetCountries();
        supplier.States = supplier.CountryId > 0 ? await _service.GetStates(supplier.CountryId) : new();
        supplier.Cities = supplier.StateId > 0 ? await _service.GetCities(supplier.StateId) : new();
    }

    public async Task<IActionResult> Details(int id)
    {
        var supplier = await _service.GetById(id);
        if (supplier == null)
            return NotFound();
        return View("~/Views/Supplier/Details.cshtml", supplier);
    }
}