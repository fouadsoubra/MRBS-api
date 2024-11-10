using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MRBS.Core.Interfaces;
using MRBS.Core.Models;
using MRBS.Core.Repositories;
using MRBS.Services.Interfaces;
using MRBS.Services;
using AutoMapper;
using Swashbuckle.AspNetCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddCors();// front end error handling
builder.Services.AddControllers();
//added for controllers
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MRBS", Version = "v1" });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //This line was added
var connection = @"Server=(localdb)\mssqllocaldb;Database=MRBS; Trusted_Connection=True; ConnectRetryCount=0";
builder.Services.AddDbContext<MrbsContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("MeetingRoomBookingSystem")));

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
