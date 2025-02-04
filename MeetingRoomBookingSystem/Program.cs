using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MRBS.Core.Interfaces;
using MRBS.Core.Models;
using MRBS.Core.Repositories;
using MRBS.Services.Interfaces;
using MRBS.Services;
using AutoMapper;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(); // Frontend error handling
builder.Services.AddControllers();

// Added for controllers
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddAutoMapper(typeof(Program));

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MRBS", Version = "v1" });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // This line was added

// Get the connection string from appsettings.json
// var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

// Use MySQL for EF Core with your connection string
builder.Services.AddDbContext<MrbsContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("MeetingRoomBookingSystem")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseCors(builder =>
{
    builder.WithOrigins("http://127.0.0.1:3000") // Replace with your actual front-end origin(s)
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
