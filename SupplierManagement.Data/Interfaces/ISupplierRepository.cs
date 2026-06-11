using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Interfaces;
public interface ISupplierRepository
{
    List<Supplier> GetAll();
}