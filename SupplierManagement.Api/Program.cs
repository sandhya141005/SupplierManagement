using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Context;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Business.Services;
using SupplierManagement.Business.Agent;
using SupplierManagement.Data.Interfaces;
using SupplierManagement.Data.Repositories;
using SupplierManagement.Api.Mappings;
using SupplierManagement.Api.Filters;
using SupplierManagement.Api.Email;
using SupplierManagement.Api.Middleware;
using Serilog;
using Hangfire;
using Hangfire.SqlServer;
using SupplierManagement.Api.AI;
var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/supplier-hub-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:dd-MMM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddControllers();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<CustomHeaderFilter>();
builder.Services.AddHttpClient<IAiService,AiService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomHeaderFilter>();
    options.Filters.Add<LoggingFilter>();
    options.Filters.Add<ResponseFilter>();
});
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();
builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IRevenueSkill, RevenueSkill>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<LoggingFilter>();
builder.Services.AddScoped<IOrderSkill, OrderSkill>();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IInventorySkill, InventorySkill>();
builder.Services.AddDbContext<AppDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/hangfire");
}
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();
app.UseHttpsRedirection();


app.Run();

