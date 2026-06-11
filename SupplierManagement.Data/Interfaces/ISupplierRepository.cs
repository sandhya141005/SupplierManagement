using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Interfaces;
public interface ISupplierRepository
{
    List<Supplier> GetAll();
    void Delete(int id);
    Supplier? GetById(int id);
    void Edit(Supplier supplier);
    }