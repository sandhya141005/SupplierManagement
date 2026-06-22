using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Context;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;

namespace SupplierManagement.Data.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;

    public Order PlaceOrder(Order order, List<(int productId, int qty)> items)
    {
        foreach (var (productId, qty) in items)
        {
            var product = _context.Products.Find(productId);
            if (product == null) throw new Exception($"Product {productId} not found");
            if (product.AvailableStock < qty)
                throw new Exception($"Insufficient stock for '{product.ProductName}'");
            product.AvailableStock -= qty;
        }
        _context.Orders.Add(order);
        _context.SaveChanges();
        return order;
    }

    public List<Order> GetOrdersByUser(int userId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public Order? GetOrderById(int orderId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.OrderId == orderId);
    }
}