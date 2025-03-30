using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using TicketAPI.Service;
using TicketAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình MongoDB từ appsettings.json

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings")
);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  // 👈 Thêm dòng này
    });
builder.Services.AddSwaggerGen();

// Đăng ký TicketService
builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<PostService>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();

app.Run();





//using CRUDOpperationMongoDB1.Models;
//using CRUDOpperationMongoDB1.Controllers;

//namespace CRUDOpperationMongoDB1
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            // Configure MongoDB settings
//            builder.Services.Configure<MongoDbSettings>(
//                builder.Configuration.GetSection("MongoDbSettings"));

//            // Register ItemService
//            builder.Services.AddSingleton<Ticket>();

//            // Add controllers
//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();
//            builder.Services.AddHttpsRedirection(options =>
//            {
//                options.HttpsPort = 443; // Or whatever port IIS Express uses
//            });
//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
