using CRUDOpperationMongoDB1.Application.Command.Customer;
using CRUDOpperationMongoDB1.Application.Command.Post;
using CRUDOpperationMongoDB1.Application.DTO;
using CRUDOpperationMongoDB1.Application.Handler.CommandHandlers;
using CRUDOpperationMongoDB1.Application.Handler.CommandHandlers.Customers;
using CRUDOpperationMongoDB1.Application.Handler.PostCommandHandlers;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Infrastructure.Repositories;
using CRUDOpperationMongoDB1.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using OfficeOpenXml;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Đọc MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Đăng ký MongoClient + Database
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDBSettings>();
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDBSettings>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Đăng ký ApplicationDbContext và Repository
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Đăng ký MediatR cho toàn bộ Assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// EPPlus config
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Cấu hình Controller & JSON
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
