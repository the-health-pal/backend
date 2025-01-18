using System.Text;
using health_pal_backend.Config;
using health_pal_backend.Repositories;
using health_pal_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.Azure.Cosmos;
using Dotnet_Core_Project.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add logging functionality
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Logging.AddEventLog();
}

// Create a logger instance
var logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

// Load the env file
DotNetEnv.Env.Load();
logger.LogInformation("Env file loaded");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")))
    };
});
logger.LogInformation("JWT Authentication Service Started");

builder.Services.AddDbContext<AppDbContext>(e => e.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")));

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new CosmosClient(
        Environment.GetEnvironmentVariable("COSMOS_ACCOUNT_ENDPOINT"), Environment.GetEnvironmentVariable("COSMOS_ACCOUNT_KEY")
        );
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserBioDataRepository, UserBioDataRepository>();

var app = builder.Build();
logger.LogInformation("Application Builder configuration complete");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
logger.LogInformation("Application Started");
