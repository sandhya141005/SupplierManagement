using SupplierManagement.Data.Entities;
namespace SupplierManagement.Data.Interfaces;
public interface IUserRepository
{
    void AddUser(User user);
    User? GetUserByEmail(string email);
    User? ValidateLogin(string email, string password);
}