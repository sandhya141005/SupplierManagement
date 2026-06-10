var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<MVCUserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
});

builder.Services.AddHttpClient<MVCAuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5165/");
});

// ❌ REMOVE this line — AddHttpClient already registers it
// builder.Services.AddScoped<MVCUserService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();