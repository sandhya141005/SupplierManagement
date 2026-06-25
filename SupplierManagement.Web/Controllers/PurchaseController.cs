using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Services;

public class PurchasesController : Controller
{
    private readonly MVCOrderService _orderService;
    private readonly PdfService _pdfservice;
    public PurchasesController(MVCOrderService orderService,PdfService pdfservice)
    {
        _orderService = orderService;
        _pdfservice=pdfservice;
    } 

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var orders = await _orderService.GetMyOrders(userId);
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetById(id);
        if (order == null) return NotFound();
        return View(order);
    }
    public async Task<IActionResult> DownloadBill(int id)
{
    var order = await _orderService.GetById(id);
    if (order == null) return NotFound();

    var userName = HttpContext.Session.GetString("UserName") ?? "Customer";
    var pdf = _pdfservice.GenerateOrderBill(order, userName);

    return File(pdf, "application/pdf", $"Bill-{order.OrderNumber}.pdf");
}
}