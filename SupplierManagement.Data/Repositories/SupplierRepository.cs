using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Context;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;

namespace SupplierManagement.Data.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;
    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }
    public List<Supplier> GetAll()
    {
        return _context.Suppliers.Include(s => s.Country).Include(s => s.State).Include(s => s.City).ToList();
    }
    public void Delete(int id)
    {
        var supp = _context.Suppliers.Find(id);
        if (supp != null)
        {
            _context.Suppliers.Remove(supp);
            _context.SaveChanges();
        }
    }
    public Supplier? GetById(int id)
    {
        return _context.Suppliers.Find(id);
    }
    public void Edit(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        _context.SaveChanges();
    }
}
