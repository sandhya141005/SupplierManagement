using SupplierManagement.Data.Context;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;
namespace SupplierManagement.Data.Repositories;
using System.Text;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    public User? GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
    public User? ValidateLogin(string email, string password)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}