using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

public class MVCLocationService
{
    private readonly HttpClient _http;
    public MVCLocationService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<SelectListItem>> GetCountries()
    {
        var result = await _http.GetFromJsonAsync<List<LocationItem>>("api/location/countries");
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