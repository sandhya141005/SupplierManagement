using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Interfaces;

public interface ICartRepository
{
    List<CartItem> GetCart(int userId);
    void AddOrUpdate(CartItem item);
    void Remove(int userId, int productId);
    void Clear(int userId);
}