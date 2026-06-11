using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupplierManagement.Web.Models.DTO;
using SupplierManagement.Web.Models;
using System.Text.Json.Serialization;

public class MVCAuthService
{
    private readonly HttpClient _http;
    public MVCAuthService(HttpClient http)
    {
        _http = http;
    }
    public async Task<(bool Success, string Error)> Register(RegisterViewModel model)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", model);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
        return (true, "");
    }
    public async Task<UserDTO?> Login(LoginViewModel model)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", model);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }
    public async Task<List<SelectListItem>> GetCountries()
    {
        var result = await _http.GetFromJsonAsync<List<LocationItem>>("api/location/countries");
        //Console.WriteLine("Countries fetched: " + result?.Count);
        return result?.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
               ?? new List<SelectListItem>();
    }

    public async Task<List<SelectListItem>> GetStates(int countryId)
    {
        var result = await _http.GetFromJsonAsync<List<LocationItem>>($"api/location/states/{countryId}");
        return result?.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList()
               ?? new List<SelectListItem>();
    }

    public async Task<List<SelectListItem>> GetCities(int stateId)
    {
        var result = await _http.GetFromJsonAsync<List<LocationItem>>($"api/location/cities/{stateId}");
        return result?.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList()
               ?? new List<SelectListItem>();
    }
}

public class LocationItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}