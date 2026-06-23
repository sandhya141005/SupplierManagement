using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Models;
namespace SupplierManagement.Web.Controllers;

public class CartController : Controller
{
    private readonly MVCCartService _cart;
    private readonly MVCOrderService _orderService;
    private readonly MVCSupplierService _supplierService;
    // private readonly MVCCartService _cart;

    public CartController(MVCCartService cart, MVCOrderService orderService, MVCSupplierService supplierService)
    {
        _cart = cart;
        _orderService = orderService;
        _supplierService = supplierService;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] CartItemViewModel item)
    {
        var (success, error) = await _cart.AddOrUpdate(item);
        if (!success) return BadRequest(error);
        var cart = await _cart.GetCart();
        return Ok(new { count = cart.Count });
    }
   
    public async Task<IActionResult> Index()
    {
        var cart = await _cart.GetCart();
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int productId)
    {
        await _cart.Remove(productId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder()
    {
        var cart = await _cart.GetCart();
        if (!cart.Any()) return RedirectToAction("Index");

        var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var req = new PlaceOrderRequest
        {
            UserId = userId,
            Items = cart.Select(c => new OrderItemViewModel
            {
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                Price = c.Price,
                Discount = c.Discount,
                Quantity = c.Quantity,
                LineTotal = c.LineTotal
            }).ToList()
        };

        var (success, error, order) = await _orderService.PlaceOrder(req);
        if (!success)
        {
            TempData["Error"] = error;
            return RedirectToAction("Index");
        }

        await _cart.Clear();
        return View("OrderConfirmation", order);
    }
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cart.GetCart();
        return Ok(cart);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateQty([FromBody] UpdateQtyRequest req)
    {
        var cart = await _cart.GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == req.ProductId);
        if (item == null) return NotFound();
        item.Quantity = req.Quantity;
        await _cart.AddOrUpdate(item);
        return Ok();
    }

    public class UpdateQtyRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}




















































