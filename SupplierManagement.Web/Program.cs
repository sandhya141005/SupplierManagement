using SupplierManagement.Web.Mappings;
using SupplierManagement.Web.Filters;
using SupplierManagement.Web.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(WebMappingProfile));
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<MVCUserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});

builder.Services.AddHttpClient<MVCAuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});
builder.Services.AddHttpClient<MVCSupplierService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});
builder.Services.AddHttpClient<MVCLocationService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});
builder.Services.AddHttpClient<MVCCartService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});

builder.Services.AddHttpClient<MVCOrderService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessionAuthFilter>();
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<PdfService>();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddHttpClient<MVCAnalyticsService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
    client.DefaultRequestHeaders.Add("X-Api-Key", "SupplierHub'26");
});
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();