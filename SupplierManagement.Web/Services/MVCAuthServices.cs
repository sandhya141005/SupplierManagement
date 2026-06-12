using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupplierManagement.Web.Models.DTO;
using SupplierManagement.Web.Models;
using System.Text.Json.Serialization;

public class MVCAuthService
{
    private readonly HttpClient _http;
    private readonly MVCLocationService _locationService;
    public MVCAuthService(HttpClient http)
    {
        _http = http;
        _locationService = new MVCLocationService(http);
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
        return await _locationService.GetCountries();
    }
    public async Task<List<SelectListItem>> GetStates(int countryId)
    {
        return await _locationService.GetStates(countryId);
    }
    public async Task<List<SelectListItem>> GetCities(int stateId)
    {
        return await _locationService.GetCities(stateId);
    }
}