using Microsoft.EntityFrameworkCore;
using BloggerLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 데이터베이스 연결 문자열
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__blogdb");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string 'blogdb' not found. Using default connection string.");
    connectionString = "Server=127.0.0.1,62106;Database=blogdb;User Id=sa;Password=j}rAUu.4-J}yRCZVhhwkXX;MultipleActiveResultSets=true;Encrypt=False";
}

Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();