using System.Net.Http.Json;
using SupplierManagement.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
public class MVCSupplierService
{
    private readonly HttpClient _http;
    private readonly IMapper _mapper;
    private readonly MVCLocationService _locationService;
    public MVCSupplierService(HttpClient http, IMapper mapper)
    {
        _http = http;
        _mapper = mapper;
        _locationService = new MVCLocationService(http);
    }
    public async Task<List<SupplierViewModel>> GetAll()
    {
        var result = await _http.GetFromJsonAsync<List<SupplierViewModel>>("api/supplier");
       // return result ?? new List<SupplierViewModel>();
       var countries=await _locationService.GetCountries();
       var stateDict=new Dictionary<int,List<SelectListItem>>();
       var cityDict=new Dictionary<int,List<SelectListItem>>();
       foreach(var res in result){
            res.Country=countries.FirstOrDefault(c=>c.Value==res.CountryId.ToString())?.Text??"";
            
            if(!stateDict.ContainsKey(res.CountryId)){
                stateDict[res.CountryId]=await _locationService.GetStates(res.CountryId);
            }
            res.State=stateDict[res.CountryId].FirstOrDefault(s=>s.Value==res.StateId.ToString())?.Text??"";
            if(!cityDict.ContainsKey(res.StateId)){
                cityDict[res.StateId]=await _locationService.GetCities(res.StateId);
            }
            res.City=cityDict[res.StateId].FirstOrDefault(c=>c.Value==res.CityId.ToString())?.Text??"";

        }
        return result;
    }
    public async Task<bool> Delete(int id)
    {
        var result = await _http.DeleteAsync($"api/supplier/{id}");
        return result.IsSuccessStatusCode;
    }
    public async Task<SupplierViewModel?> GetById(int id)
    {
        var result = await _http.GetFromJsonAsync<SupplierViewModel>($"api/supplier/{id}");
        return result;
    }
    public async Task<(bool Success, string Error)> Edit(int id, SupplierViewModel supplier)
{
    var result = await _http.PutAsJsonAsync($"api/supplier/{supplier.SupplierId}", supplier);
    if (!result.IsSuccessStatusCode)
    {
        var error = await result.Content.ReadAsStringAsync();
        return (false, error);
    }
    return (true, "");
}
    public async Task<(bool Success, String Error)> Add(SupplierViewModel supplier)
    {
        var result = await _http.PostAsJsonAsync("api/supplier", supplier);
        if (!result.IsSuccessStatusCode)
        {
            var error = await result.Content.ReadAsStringAsync();
            return (false, error);
        }
        return (true, "");
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