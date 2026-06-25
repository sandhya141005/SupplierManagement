using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Models;
using ClosedXML.Excel;
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
        return View("SupplierForm", supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SupplierViewModel supplier)
    {
        supplier.PaymentMethodsAllowed = string.Join(", ", supplier.SelectedPaymentMethods);

        supplier.Products = supplier.Products
            .Where(p => !string.IsNullOrWhiteSpace(p.ProductName))
            .ToList();
        if (!ModelState.IsValid)
        {
            await ReloadDropdowns(supplier);
            return View("SupplierForm", supplier);
        }
        var (success, error) = await _service.Edit(id, supplier);
        if (!success)
        {
            ModelState.AddModelError("", $"Failed to update supplier: {error}");
            await ReloadDropdowns(supplier);
            return View("SupplierForm", supplier);
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Add()
    {

        var model = new SupplierViewModel { Countries = await _service.GetCountries() };
        return View("SupplierForm", model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SupplierViewModel supplier)
    {

        supplier.PaymentMethodsAllowed = string.Join(", ", supplier.SelectedPaymentMethods);

        supplier.Products = supplier.Products.Where(p => !string.IsNullOrWhiteSpace(p.ProductName)).ToList();

        if (!ModelState.IsValid)
        {
            await ReloadDropdowns(supplier);
            return View("SupplierForm", supplier);
        }

        var (success, error) = await _service.Add(supplier);
        if (!success)
        {
            ModelState.AddModelError("", error);
            await ReloadDropdowns(supplier);
            return View("SupplierForm", supplier);
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
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Testing API exception filter");
    }

    public async Task<IActionResult> DownloadExcel()
    {
        var suppliers = await _service.GetAll();
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Suppliers");
        var headers = new[]
        {
        "Company Name", "Catalog Type", "Total Products",
        "Payment Methods", "Created Date", "Location", "Contact"
    };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4E342E");
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
        int row = 2;
        foreach (var s in suppliers)
        {
            ws.Cell(row, 1).Value = s.CompanyName;
            ws.Cell(row, 2).Value = s.CatalogType;
            ws.Cell(row, 3).Value = s.TotalProducts;
            ws.Cell(row, 4).Value = s.PaymentMethodsAllowed;
            ws.Cell(row, 5).Value = s.CreatedDate;
            ws.Cell(row, 6).Value = $"{s.City}, {s.State}, {s.Country}";
            ws.Cell(row, 7).Value = s.ContactNo ?? "";
            if (row % 2 == 0)
            {
                ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromHtml("#FAF7F5");
            }
            row++;
        }
        ws.Columns().AdjustToContents();
        var range = ws.Range(1, 1, row - 1, headers.Length);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.OutsideBorderColor = XLColor.FromHtml("#D7CCC8");
        range.Style.Border.InsideBorderColor = XLColor.FromHtml("#D7CCC8");
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Suppliers-{DateTime.Now:dd-MMM-yyyy}.xlsx");
    }
}