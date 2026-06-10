using SupplierManagement.Data.Entities;
namespace SupplierManagement.Business.Interfaces;
public interface IUserService
{
    void Register(User user);
    string Encode(string password);
    User? Login(string email, string password);
}