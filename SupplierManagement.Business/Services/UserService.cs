using System.Text;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Data.Entities;
using SupplierManagement.Data.Interfaces;
namespace SupplierManagement.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo)
    {
        _repo=repo;
    }
    private string Encode(string password)
    {
        byte[] bytes=Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(bytes);
    }
    public void Register(User user)
    {
        if (_repo.GetUserByEmail(user.Email) != null)
        {
            throw new Exception("Email already exists");
        }
        user.Password=Encode(user.Password);
        _repo.AddUser(user);
    }
     public User? Login(string email, string password)
    {
        string encodedPassword = Encode(password);

        return _repo.ValidateLogin(email,encodedPassword);
    }

    string IUserService.Encode(string password)
    {
        return Encode(password);
    }
}