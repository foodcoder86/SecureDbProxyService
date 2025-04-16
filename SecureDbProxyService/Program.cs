using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Securely get the database connection string
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure authentication (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://your-identity-provider.com";  // Your Identity Provider
        options.Audience = "secure-db-api";  // Your API Audience
        options.RequireHttpsMetadata = true; // Ensures HTTPS
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])
            ),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadOnlyAccess", policy => policy.RequireRole("ReadOnly"));
    options.AddPolicy("ReadWriteAccess", policy => policy.RequireRole("ReadWrite"));
});

builder.Services.AddControllers();

// ✅ Force HTTPS & Enable Logging
builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // Enforces HTTPS
    });
});

// ✅ Enable Logging & Exception Handling
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();  // Logs to Console
    logging.AddDebug();    // Logs to Debug Output
});

var app = builder.Build();

// ✅ Global Exception Handling
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(error.Error, "Unhandled exception occurred");

            await context.Response.WriteAsync($"{{\"error\": \"An unexpected error occurred\"}}");
        }
    });
});

// ✅ Force HTTPS
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
