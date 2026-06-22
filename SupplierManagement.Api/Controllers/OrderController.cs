using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Api.DTO;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using AutoMapper;
namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IMapper _mapper;

    public OrderController(IOrderService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    [HttpPost]

    [HttpPost]
    public IActionResult PlaceOrder(PlaceOrderDTO dto)
    {
        try
        {
            var order = _mapper.Map<Order>(dto);  // ← replaces all manual mapping
            var stockItems = dto.Items.Select(i => (i.ProductId, i.Quantity)).ToList();
            _service.PlaceOrder(order, stockItems);
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