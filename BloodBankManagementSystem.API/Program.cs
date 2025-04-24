// // using BloodBankManagementSystem.Infrastructure.Data;
// // using BloodBankManagementSystem.Core.Interfaces;
// // using BloodBankManagementSystem.Infrastructure.Repositories;
// // using BloodBankManagementSystem.API.Services;
// // using BloodBankManagementSystem.Core.Models;
// // using Microsoft.EntityFrameworkCore;

// // var builder = WebApplication.CreateBuilder(args);

// // //Register EmailSettings
// // builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("SmtpSettings"));
// // //Register Email Service
// // builder.Services.AddScoped<IEmailService, EmailService>();


// // // Add services to the container.
// // builder.Services.AddControllers();
// // builder.Services.AddEndpointsApiExplorer();
// // builder.Services.AddSwaggerGen();
// // builder.Services.AddHttpClient();
// // var configuration = builder.Configuration;
// // builder.Services.AddSingleton<IConfiguration>(configuration);

// // // Configure DbContext with SQL Server
// // builder.Services.AddDbContext<AppDbContext>(options =>
// //     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
// //     sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// // // Register Reposittories
// // builder.Services.AddScoped<IUserRepository, UserRepository>();
// // builder.Services.AddScoped<IPatientRepository, PatientRepository>();
// // builder.Services.AddScoped<IDonorRepository, DonorRepository>();

// // // Configure CORS properly
// // builder.Services.AddCors(options =>
// // {
// //     options.AddPolicy("AllowAll", policy =>
// //         policy.AllowAnyOrigin()
// //               .AllowAnyMethod()
// //               .AllowAnyHeader());
// // });


// // builder.Services.AddScoped<JwtTokenService>();


// // var app = builder.Build();

// // app.UseAuthentication();
// // app.UseAuthorization();

// // // Use CORS Middleware (before Authorization)
// // app.UseCors("AllowAll");

// // // Enable Swagger only in Development
// // if (app.Environment.IsDevelopment())
// // {
// //     app.UseSwagger();
// //     app.UseSwaggerUI();
// // }

// // app.UseHttpsRedirection();
// // app.UseAuthorization();
// // app.MapControllers();

// // app.Run();



// using BloodBankManagementSystem.Infrastructure.Data;
// using BloodBankManagementSystem.Core.Interfaces;
// using BloodBankManagementSystem.Infrastructure.Repositories;
// using BloodBankManagementSystem.API.Services;
// using BloodBankManagementSystem.Core.Models;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // ✅ Load Configuration
// var configuration = builder.Configuration;
// builder.Services.AddSingleton<IConfiguration>(configuration);

// // ✅ Register Email Settings
// builder.Services.Configure<EmailSettings>(configuration.GetSection("SmtpSettings"));
// builder.Services.AddScoped<IEmailService, EmailService>();

// // ✅ Configure Database (SQL Server)
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
//     sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// // ✅ Register Repositories
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IPatientRepository, PatientRepository>();
// builder.Services.AddScoped<IDonorRepository, DonorRepository>();

// // ✅ Register JWT Token Service
// builder.Services.AddSingleton<JwtTokenService>();

// // ✅ Configure JWT Authentication
// var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? configuration["Jwt:Key"];
// var jwtIssuer = configuration["Jwt:Issuer"];
// var jwtAudience = configuration["Jwt:Audience"]; // Fetch audience

// if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
// {
//     Console.WriteLine("⚠️ Warning: JWT configuration is missing!");
//     throw new InvalidOperationException("JWT configuration is missing in environment variables or appsettings.json");
// }

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = jwtIssuer,
//             ValidAudience = jwtAudience, // Now uses correct audience
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//         };
//     });

// // ✅ Configure CORS (Restrict for Production)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader());

//     options.AddPolicy("AllowSpecific", policy =>
//         policy.WithOrigins("http://localhost:5097") // Change this in production
//               .AllowAnyMethod()
//               .AllowAnyHeader());
// });

// // ✅ Add Controllers & Swagger
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddHttpClient();

// var app = builder.Build();

// // ✅ Enable Authentication & Authorization Middleware
// app.UseAuthentication();
// app.UseAuthorization();

// // ✅ Use CORS Middleware (before Authorization)
// app.UseCors("AllowAll"); // Change to "AllowSpecific" in production

// // ✅ Enable Swagger only in Development
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.MapControllers();
// app.Run();








using BloodBankManagementSystem.Infrastructure.Data;
using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Infrastructure.Repositories;
using BloodBankManagementSystem.API.Services;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load Configuration
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);

// ✅ Environment-Based Configurations
var isDevelopment = builder.Environment.IsDevelopment();
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? configuration["Jwt:Key"];
var jwtIssuer = configuration["Jwt:Issuer"];
var jwtAudience = configuration["Jwt:Audience"];
var dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration is missing!");
}

// ✅ Register Email Settings
builder.Services.Configure<EmailSettings>(configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// ✅ Configure Database (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(dbConnection, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// ✅ Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();
builder.Services.AddScoped<IBloodRequestRepository, BloodRequestRepository>();


// ✅ Register JWT Token Service
builder.Services.AddSingleton<JwtTokenService>();

// ✅ Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ✅ Configure CORS (Restrict in Production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

    options.AddPolicy("AllowSpecific", policy =>
        policy.WithOrigins(configuration["AllowedHosts"] ?? "http://localhost:5097") // Change for Production
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ✅ Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blood Bank API", Version = "v1" });

    // ✅ Add JWT Authorization to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}' to authenticate"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// ✅ Build Application
var app = builder.Build();

// ✅ Enable Middleware (Correct Order)
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(isDevelopment ? "AllowAll" : "AllowSpecific"); // Restrict CORS in Production
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
