using System.Text.Json;
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
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<SubmissionService>();
builder.Services.AddScoped<HomeworkService>();

builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IRepository<Group>, GroupRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<SubmissionRepository>();
builder.Services.AddScoped<HomeworkRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    }); // для русских букв в отображении, но можно будет убрать потом, просто вместо букв бутут \u9347 или типа того



var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
