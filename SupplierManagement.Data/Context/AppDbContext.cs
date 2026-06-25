using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Price) .HasPrecision(18, 2);//absence of these causes arith overflow/unwanted truncation/store tyoe errors
        modelBuilder.Entity<Product>().Property(p => p.Discount).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(o => o.Price) .HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(o => o.Discount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(o => o.LineTotal).HasPrecision(18, 2);
        modelBuilder.Entity<CartItem>().Property(c => c.Price).HasPrecision(18, 2);
        modelBuilder.Entity<CartItem>().Property(c => c.Discount).HasPrecision(18, 2);
        modelBuilder.Entity<Product>().HasOne(p => p.Supplier).WithMany(s => s.Products).HasForeignKey(p => p.SupplierId); //nav prop error
    }
}