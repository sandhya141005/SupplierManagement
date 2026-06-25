using System.Net.Http.Json;
using SupplierManagement.Web.Models;
public class MVCAnalyticsService
{
    private readonly HttpClient _http;
    public MVCAnalyticsService(HttpClient http) => _http = http;

    public async Task<List<SupplierRevenueViewModel>> GetSupplierRevenue()
    {
        var result = await _http.GetFromJsonAsync<List<SupplierRevenueViewModel>>("api/analytics/supplier-revenue");
        return result ?? new();
    }

    public async Task<List<CountrySalesViewModel>> GetCountrySales()
    {
        var result = await _http.GetFromJsonAsync<List<CountrySalesViewModel>>("api/analytics/country-sales");
        return result ?? new();
    }
}