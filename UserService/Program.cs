using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Controllers;
using UserService.Data;
using UserService.Managers;
using UserService.Interfaces;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);







// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<CustomerController>();
builder.Services.AddHostedService<UserProcessingService>();

// Register the DbContext with dependency injection to access the database context throughout the application.
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MyDbConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MyDbConnection"))));

// Register additional managers here

builder.Services.AddScoped<CustomerManager>();
builder.Services.AddScoped<AddressManager>();


// Register the repository implementations for dependency injection

builder.Services.AddScoped<ICustomerManager, CustomerManager>();
builder.Services.AddScoped<IAddressManager, AddressManager>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
