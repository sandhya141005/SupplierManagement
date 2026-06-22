using Microsoft.AspNetCore.Mvc;

public class PurchasesController : Controller
{
    private readonly MVCOrderService _orderService;
    public PurchasesController(MVCOrderService orderService) => _orderService = orderService;

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
}