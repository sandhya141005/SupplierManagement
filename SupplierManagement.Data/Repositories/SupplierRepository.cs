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
        return _context.Suppliers.Include(s => s.Country).Include(s => s.State).Include(s => s.City).Include(s => s.Products).ToList();
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
        return _context.Suppliers.Include(s => s.Country).Include(s => s.State).Include(s => s.City).Include(s => s.Products).FirstOrDefault(s => s.SupplierId == id);
    }
    public void Edit(Supplier supplier)
    {
        var existing = _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefault(s => s.SupplierId == supplier.SupplierId);

        if (existing == null) return;

        // Remove products no longer present
        var incomingIds = supplier.Products.Where(p => p.ProductId > 0).Select(p => p.ProductId).ToHashSet();
        var toDelete = existing.Products.Where(p => !incomingIds.Contains(p.ProductId)).ToList();
        foreach (var p in toDelete)
        {
            _context.Products.Remove(p);
        }

        // Update existing / add new
        foreach (var p in supplier.Products)
        {
            if (p.ProductId > 0)
            {
                var existingProduct = existing.Products.FirstOrDefault(ep => ep.ProductId == p.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = p.ProductName;
                    existingProduct.Category = p.Category;
                    existingProduct.Price = p.Price;
                    existingProduct.Discount = p.Discount;
                    existingProduct.AvailableStock = p.AvailableStock;
                }
            }
            else
            {
                p.SupplierId = existing.SupplierId;
                p.CreatedDate = DateTime.Now;
                existing.Products.Add(p);
            }
        }

        // Update supplier scalar fields
        existing.CompanyName = supplier.CompanyName;
        existing.TotalProducts = supplier.TotalProducts;
        existing.CatalogType = supplier.CatalogType;
        existing.PaymentMethodsAllowed = supplier.PaymentMethodsAllowed;
        existing.ContactNo = supplier.ContactNo;
        existing.CountryId = supplier.CountryId;
        existing.StateId = supplier.StateId;
        existing.CityId = supplier.CityId;

        _context.SaveChanges();
    }
    public void Add(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        _context.SaveChanges();
    }
}
