using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
      }
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }

    public DbSet<City> Cities { get; set; }
}