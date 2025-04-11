using Cs_backend.Database;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services;
using Microsoft.EntityFrameworkCore;
using Task = Cs_backend.Models.Task;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<TaskRepository>();


var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
