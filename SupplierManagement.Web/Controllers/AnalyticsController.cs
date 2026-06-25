using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Models;

public class AnalyticsController : Controller
{
    private readonly MVCAnalyticsService _service;
    public AnalyticsController(MVCAnalyticsService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var model = new AnalyticsViewModel
        {
            SupplierRevenue = await _service.GetSupplierRevenue(),
            CountrySales = await _service.GetCountrySales()
        };
        return View(model);
    }
}