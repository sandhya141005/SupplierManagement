using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using AutoMapper;
using SupplierManagement.Api.Email;
using SupplierManagement.Data.Context;
using Hangfire;
namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly AppDbContext _context;

    public OrderController(IOrderService service, IMapper mapper, IEmailService emailService, AppDbContext context)
    {
        _service = service;
        _mapper = mapper;
        _context=context;
        _emailService=emailService;
    }
    [HttpPost]

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(PlaceOrderDTO dto)
    {
        try
        {
            var order = _mapper.Map<Order>(dto); 
            var stockItems = dto.Items.Select(i => (i.ProductId, i.Quantity)).ToList();
            _service.PlaceOrder(order, stockItems);
             var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId);
        if (user != null)
        { BackgroundJob.Enqueue<IEmailService>(emailService =>
                emailService.SendOrderConfirmationAsync(
                    user.Email,
                    user.FirstName,
                    order.OrderNumber,
                    order.TotalAmount,
                    order.OrderDate));
        }
            return Ok(_mapper.Map<OrderDTO>(order));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetByUser(int userId)
    {
        var orders = _service.GetOrdersByUser(userId);
        return Ok(_mapper.Map<List<OrderDTO>>(orders));
    }

    [HttpGet("{orderId}")]
    public IActionResult GetById(int orderId)
    {
        var order = _service.GetOrderById(orderId);
        if (order == null) return NotFound();
        return Ok(_mapper.Map<OrderDTO>(order));
    }
}