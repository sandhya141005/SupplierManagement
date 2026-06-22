using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;

namespace SupplierManagement.Business.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
    public Order PlaceOrder(Order order, List<(int productId, int qty)> items) => _repo.PlaceOrder(order, items);
    public List<Order> GetOrdersByUser(int userId) => _repo.GetOrdersByUser(userId);
    public Order? GetOrderById(int orderId) => _repo.GetOrderById(orderId);
}