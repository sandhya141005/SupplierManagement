using SupplierManagement.Data.Entities;
namespace SupplierManagement.Business.Interfaces;

public interface ISupplierService
{
    List<Supplier> GetAll();
}