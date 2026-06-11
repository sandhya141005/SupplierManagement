var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<MVCUserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
});

builder.Services.AddHttpClient<MVCAuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
});
builder.Services.AddHttpClient<MVCSupplierService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
});
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();