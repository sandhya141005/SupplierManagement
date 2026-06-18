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

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasPrecision(18, 2);

    modelBuilder.Entity<Product>()
        .Property(p => p.Discount)
        .HasPrecision(18, 2);

/*
    modelBuilder.Entity<Supplier>()
        .HasOne(s => s.Country)
        .WithMany()
        .HasForeignKey(s => s.CountryId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Supplier>()
        .HasOne(s => s.State)
        .WithMany()
        .HasForeignKey(s => s.StateId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Supplier>()
        .HasOne(s => s.City)
        .WithMany()
        .HasForeignKey(s => s.CityId)
        .OnDelete(DeleteBehavior.NoAction);
        */
    modelBuilder.Entity<Product>()
    .HasOne(p => p.Supplier)
    .WithMany(s => s.Products)
    .HasForeignKey(p => p.SupplierId);
}}