using System.Net.Http.Json;
using SupplierManagement.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.Json;
using SupplierManagement.Web.MVCDTO;
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
    private void FormatCreatedDate(SupplierViewModel model)
    {
        if (model.CreatedDateRaw.HasValue)
        {
            model.CreatedDate =
                model.CreatedDateRaw.Value.ToString(
                    "dd-MMM-yyyy",
                    CultureInfo.InvariantCulture);
        }
    }
    private SupplierDTO BuildSupplierDTO(
    SupplierViewModel model,
    DateTime createdDate)
    {
        return new SupplierDTO
        {
            SupplierId = model.SupplierId,
            CompanyName = model.CompanyName,
            TotalProducts = model.TotalProducts,
            CatalogType = model.CatalogType,
            PaymentMethodsAllowed = model.PaymentMethodsAllowed,
            ContactNo = model.ContactNo,
            CountryId = model.CountryId,
            StateId = model.StateId,
            CityId = model.CityId,
            CreatedDate = createdDate,
            DeletedProductIds = model.DeletedProductIds,

            Products = model.Products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Category = p.Category,
                Price = p.Price,
                Discount = p.Discount,
                AvailableStock = p.AvailableStock,
                SupplierId = p.SupplierId,
                CreatedDate = p.CreatedDate ?? DateTime.Now
            }).ToList()
        };
    }
    public async Task<List<SupplierViewModel>> GetAll()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result =
            await _http.GetFromJsonAsync<List<SupplierViewModel>>(
                "api/supplier",
                options);

        if (result == null)
            return new();

        var countries = await _locationService.GetCountries();

        var stateDict = new Dictionary<int, List<SelectListItem>>();
        var cityDict = new Dictionary<int, List<SelectListItem>>();

        foreach (var res in result)
        {
            FormatCreatedDate(res);

            res.Country =
                countries.FirstOrDefault(
                    c => c.Value == res.CountryId.ToString())?.Text ?? "";

            if (!stateDict.ContainsKey(res.CountryId))
            {
                stateDict[res.CountryId] =
                    await _locationService.GetStates(res.CountryId);
            }

            res.State =
                stateDict[res.CountryId]
                    .FirstOrDefault(
                        s => s.Value == res.StateId.ToString())?.Text ?? "";

            if (!cityDict.ContainsKey(res.StateId))
            {
                cityDict[res.StateId] =
                    await _locationService.GetCities(res.StateId);
            }

            res.City =
                cityDict[res.StateId]
                    .FirstOrDefault(
                        c => c.Value == res.CityId.ToString())?.Text ?? "";
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
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result =
            await _http.GetFromJsonAsync<SupplierViewModel>(
                $"api/supplier/{id}",
                options);

        if (result == null)
            return null;

        FormatCreatedDate(result);

        var countries = await _locationService.GetCountries();

        result.Country =
            countries.FirstOrDefault(
                c => c.Value == result.CountryId.ToString())?.Text ?? "";

        var states =
            await _locationService.GetStates(result.CountryId);

        result.State =
            states.FirstOrDefault(
                s => s.Value == result.StateId.ToString())?.Text ?? "";

        var cities =
            await _locationService.GetCities(result.StateId);

        result.City =
            cities.FirstOrDefault(
                c => c.Value == result.CityId.ToString())?.Text ?? "";

        return result;
    }
    public async Task<(bool Success, string Error)> Edit(int id, SupplierViewModel model)
    {
        if (!DateTime.TryParseExact(model.CreatedDate, "dd-MMM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var createdDate))
            return (false, "Invalid date format. Use dd-Mon-yyyy e.g. 11-Jun-2026");

        var dto = BuildSupplierDTO(model, createdDate);
        var response = await _http.PutAsJsonAsync($"api/supplier/{id}", dto);
        if (!response.IsSuccessStatusCode)
            return (false, await response.Content.ReadAsStringAsync());
        return (true, "");
    }
    public async Task<(bool Success, string Error)> Add(SupplierViewModel model)
    {
        if (!DateTime.TryParseExact(model.CreatedDate, "dd-MMM-yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var createdDate))
            return (false, "Invalid date format. Use dd-Mon-yyyy e.g. 11-Jun-2026");

        var dto = BuildSupplierDTO(model, createdDate);
        var response = await _http.PostAsJsonAsync("api/supplier", dto);
        if (!response.IsSuccessStatusCode)
            return (false, await response.Content.ReadAsStringAsync());
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