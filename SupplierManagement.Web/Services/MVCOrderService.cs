using System.Net.Http.Json;
using SupplierManagement.Web.Models;

public class MVCOrderService
{
    private readonly HttpClient _http;

    public MVCOrderService(HttpClient http) => _http = http;

    public async Task<(bool Success, string Error, OrderViewModel? Order)> PlaceOrder(PlaceOrderRequest req)
    {
        var response = await _http.PostAsJsonAsync("api/order", req);
        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            return (false, err, null);
        }
        var order = await response.Content.ReadFromJsonAsync<OrderViewModel>();
        return (true, "", order);
    }

    public async Task<List<OrderViewModel>> GetMyOrders(int userId)
    {
        var result = await _http.GetFromJsonAsync<List<OrderViewModel>>($"api/order/user/{userId}");
        return result ?? new();
    }

    public async Task<OrderViewModel?> GetById(int orderId)
    {
        return await _http.GetFromJsonAsync<OrderViewModel>($"api/order/{orderId}");
    }
}

public class PlaceOrderRequest
{
    public int UserId { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new();
}