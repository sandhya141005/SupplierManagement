using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Context;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;
using SupplierManagement.Data.DTO;
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
    public async Task<List<OrderSummaryDTO>> GetOrderSummaryAsync()
    {
        return await (
            from o in _context.Orders
            from oi in o.OrderItems
            join p in _context.Products
                on oi.ProductId equals p.ProductId
            join s in _context.Suppliers
                on p.SupplierId equals s.SupplierId
            group new { o, oi, p, s } by new
            {
                p.ProductId,
                p.ProductName,
                p.Category,
                p.AvailableStock,
                s.CompanyName
            }
            into g
            select new OrderSummaryDTO
            {
                ProductName = g.Key.ProductName,
                Category = g.Key.Category,
                CompanyName = g.Key.CompanyName,
                UnitsSold = g.Sum(x => x.oi.Quantity),
                TotalOrders = g.Select(x => x.o.OrderId).Distinct().Count(),
                Revenue = g.Sum(x => x.oi.LineTotal),
                CurrentStock = g.Key.AvailableStock,
                AvgUnitsPerOrder = g.Sum(x => x.oi.Quantity) / (double)g.Select(x => x.o.OrderId).Distinct().Count(),
                AvgDailySales = g.Sum(x => x.oi.Quantity) /
                               Math.Max(EF.Functions.DateDiffDay(g.Min(x => x.o.OrderDate), g.Max(x => x.o.OrderDate)), 1),
                EstimatedDaysUntilStockout = g.Key.AvailableStock /
                    Math.Max(g.Sum(x => x.oi.Quantity) / (double)Math.Max(EF.Functions.DateDiffDay(g.Min(x => x.o.OrderDate), g.Max(x => x.o.OrderDate)), 1), 1)
            }).ToListAsync();
    }
}

