using SupplierManagement.Data.Entities;
namespace SupplierManagement.Business.Interfaces;

public interface ISupplierService
{
    List<Supplier> GetAll();
    void Delete(int id);
    Supplier? GetById(int id);
    void Edit(Supplier supplier);

}