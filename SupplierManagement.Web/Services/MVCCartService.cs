using System.Net.Http.Json;
using SupplierManagement.Web.Models;

public class MVCCartService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _accessor;

    public MVCCartService(HttpClient http, IHttpContextAccessor accessor)
    {
        _http = http;
        _accessor = accessor;
    }

    private int UserId => int.Parse(
        _accessor.HttpContext!.Session.GetString("UserId") ?? "0");

    public async Task<List<CartItemViewModel>> GetCart() =>
        await _http.GetFromJsonAsync<List<CartItemViewModel>>($"api/cart/{UserId}") ?? new();

    public async Task<(bool Success, string Error)> AddOrUpdate(CartItemViewModel item)
    {
        item.UserId = UserId;
        var response = await _http.PostAsJsonAsync("api/cart", item);
        if (!response.IsSuccessStatusCode)
            return (false, await response.Content.ReadAsStringAsync());
        return (true, "");
    }

    public async Task Remove(int productId) =>
        await _http.DeleteAsync($"api/cart/{UserId}/{productId}");

    public async Task Clear() =>
        await _http.DeleteAsync($"api/cart/{UserId}");
}