using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;
using SupplierManagement.Api.DTO;
using AutoMapper;
namespace SupplierManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;

    public CartController(ICartRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    [HttpGet("{userId}")]
    public IActionResult GetCart(int userId)
    {
        var items = _repo.GetCart(userId);
        return Ok(_mapper.Map<List<CartItemDTO>>(items));
    }

    [HttpPost]
    public IActionResult AddOrUpdate(CartItemDTO dto)
    {
        if (dto.Quantity < 1 || dto.Quantity > dto.AvailableStock)
            return BadRequest("Quantity exceeds available stock.");
        var item = _mapper.Map<CartItem>(dto);
        _repo.AddOrUpdate(item);
        return Ok(_repo.GetCart(dto.UserId).Count);
    }
    [HttpDelete("{userId}/{productId}")]
    public IActionResult Remove(int userId, int productId)
    {
        _repo.Remove(userId, productId);
        return Ok();
    }

    [HttpDelete("{userId}")]
    public IActionResult Clear(int userId)
    {
        _repo.Clear(userId);
        return Ok();
    }
}