using CRUDOpperationMongoDB1.Application.Handler.CommandHandlers;
using CRUDOpperationMongoDB1.Application.Interfaces;
using CRUDOpperationMongoDB1.Data;
using CRUDOpperationMongoDB1.Domain.Entities;
using CRUDOpperationMongoDB1.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình MongoDB từ appsettings.json
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Đăng ký IApplicationDbContext và TicketRepository
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Thiết lập License EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Hoặc LicenseContext.Commercial nếu bạn có giấy phép

// Thêm các dịch vụ vào container DI
builder.Services.AddControllersWithViews();

// Cấu hình MediatR - tự động quét assembly chứa handler
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateTicketCommandHandler).Assembly));

// Cấu hình controller & JSON options để hỗ trợ Enum Serialization
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Cấu hình Swagger cho testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Nếu bạn cần hỗ trợ CORS (cross-origin requests)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Cấu hình middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Bật CORS nếu cần
app.UseCors("AllowAll");

app.UseHttpsRedirection(); // Chuyển hướng HTTP sang HTTPS
app.UseAuthorization(); // Xử lý xác thực và ủy quyền

app.MapControllers(); // Cấu hình routing cho controllers

app.Run();
