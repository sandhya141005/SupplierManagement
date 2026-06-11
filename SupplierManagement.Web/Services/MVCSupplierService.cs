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
        var result = await _http.GetFromJsonAsync<List<SupplierViewModel>>("api/supplier");
        return result ?? new List<SupplierViewModel>();
    }
    public async Task<bool> Delete(int id)
    {
        var result= await _http.DeleteAsync($"api/supplier/{id}");
        return result.IsSuccessStatusCode;
    }
    public async Task<SupplierViewModel?> GetById(int id)
    {
        var result=await _http.GetFromJsonAsync<SupplierViewModel>($"api/supplier/{id}");
        return result;
    }
    public async Task<bool> Edit(int id,SupplierViewModel supplier)
    {
        var result = await _http.PutAsJsonAsync($"api/supplier/{supplier.SupplierId}", supplier);
        return result.IsSuccessStatusCode;
    }
}