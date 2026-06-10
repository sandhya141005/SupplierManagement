namespace SupplierManagement.Api.DTO;
public class RegisterDTO
{
    public string Email { get; set; } = "";

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Gender { get; set; } = "";

    public string Password { get; set; } = "";

    public string? ContactNo { get; set; }

    public bool IsAdmin { get; set; } 

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }
}