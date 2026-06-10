namespace SupplierManagement.Data.Entities;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Role { get; set; } = "";
    public string Password { get; set; } = "";
    public string ContactNo { get; set; } = "";
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
}