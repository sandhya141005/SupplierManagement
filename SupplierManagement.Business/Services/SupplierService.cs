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
}