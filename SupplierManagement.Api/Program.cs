using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data.Context;
using SupplierManagement.Business.Interfaces;
using SupplierManagement.Business.Services;
using SupplierManagement.Data.Interfaces;
using SupplierManagement.Data.Repositories;
using SupplierManagement.Api.Mappings;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(ApiMappingProfile));
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();


app.Run();

