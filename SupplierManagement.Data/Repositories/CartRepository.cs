using SupplierManagement.Data.Context;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;

namespace SupplierManagement.Data.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context) => _context = context;

    public List<CartItem> GetCart(int userId) =>
        _context.CartItems.Where(c => c.UserId == userId).ToList();

    public void AddOrUpdate(CartItem item)
    {
        var existing = _context.CartItems
            .FirstOrDefault(c => c.UserId == item.UserId && c.ProductId == item.ProductId);
        if (existing != null)
            existing.Quantity = item.Quantity;
        else
            _context.CartItems.Add(item);
        _context.SaveChanges();
    }

    public void Remove(int userId, int productId)
    {
        var item = _context.CartItems
            .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
    }

    public void Clear(int userId)
    {
        var items = _context.CartItems.Where(c => c.UserId == userId).ToList();
        _context.CartItems.RemoveRange(items);
        _context.SaveChanges();
    }
}