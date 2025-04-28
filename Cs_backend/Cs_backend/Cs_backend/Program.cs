using System.Text;
using System.Text.Json;
using Cs_backend;
using Cs_backend.Database;
using Cs_backend.Models;
using Cs_backend.Repositories;
using Cs_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

#region Auth
DotNetEnv.Env.Load();
var jwtSettings = new JwtSettings
{
    Key = Environment.GetEnvironmentVariable("JWT_KEY"),
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
    ExpiresInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES") ?? "60")
};

builder.Services.Configure<JwtSettings>(options =>
{
    options.Key = jwtSettings.Key;
    options.Issuer = jwtSettings.Issuer;
    options.Audience = jwtSettings.Audience;
    options.ExpiresInMinutes = jwtSettings.ExpiresInMinutes;
});


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 498;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 498,
                    message = "Token is missing or invalid"
                }));
            }
        };
    });
builder.Services.AddAuthorization();

#endregion

#region Cors
// Эдик просил
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostFrontend", policy =>
    {
        policy.WithOrigins("http://130.193.59.231")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#endregion

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<SubmissionService>();
builder.Services.AddScoped<HomeworkService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IRepository<Group>, GroupRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<SubmissionRepository>();
builder.Services.AddScoped<HomeworkRepository>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<AuthRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder =
            System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    }); // для русских букв в отображении, но можно будет убрать потом, просто вместо букв бутут \u9347 или типа того


var app = builder.Build();

app.UseCors("AllowLocalhostFrontend");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();