using System.Net.Http.Json;
using SupplierManagement.Web.Models.DTO;
public class MVCUserService
{
    private readonly HttpClient _http;

    public MVCUserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UserDTO>> GetUsers()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<UserDTO>>("api/auth");
            Console.WriteLine(result?.Count);
            return result ?? new List<UserDTO>(); ;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
            return new List<UserDTO>();
        }
    }
}