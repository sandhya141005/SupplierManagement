using System.Net.Http.Json;
using SupplierManagement.Web.Models;

public class MVCSupplierService
{
    private readonly HttpClient _http;

    public MVCSupplierService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<SupplierViewModel>> GetAll()
    {
        var result=await _http.GetFromJsonAsync<List<SupplierViewModel>>("api/supplier");
        return result??new List<SupplierViewModel>();
    }
}