using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;

namespace SupplierManagement.Business.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repo;
    public SupplierService(ISupplierRepository repo)
    {
        _repo = repo;
    }
    public List<Supplier> GetAll()
    {
        return _repo.GetAll();
    }
    public void Delete(int id)
    {
        _repo.Delete(id);
    }
    public Supplier? GetById(int id)
    {
        return _repo.GetById(id);
    }
    public void Edit(Supplier supplier)
    {
        _repo.Edit(supplier);
    }
}