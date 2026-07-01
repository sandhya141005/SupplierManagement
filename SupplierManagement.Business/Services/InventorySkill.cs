
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Context;
using SupplierManagement.Data.DTO;
namespace SupplierManagement.Business.Services
{
    public class InventorySkill : IInventorySkill
    {
        private readonly AppDbContext _context;

        public InventorySkill(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductInventoryDTO>> GetStocksAsync()
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .OrderBy(p => p.AvailableStock)
                .Select(p => new ProductInventoryDTO
                {
                    ProductName = p.ProductName,
                    CompanyName = p.Supplier.CompanyName,
                    StockQuantity = p.AvailableStock
                })
                .ToListAsync();
        }
    }
}