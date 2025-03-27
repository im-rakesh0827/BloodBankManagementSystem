using BloodBankManagementSystem.Infrastructure.Data;
using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Infrastructure.Repositories;
using BloodBankManagementSystem.API.Services;
using BloodBankManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Register EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("SmtpSettings"));
//Register Email Service
builder.Services.AddScoped<IEmailService, EmailService>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);

// Configure DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// Register Reposittories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();

// Configure CORS properly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});



var app = builder.Build();

// Use CORS Middleware (before Authorization)
app.UseCors("AllowAll");

// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
