﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using SafeMum.API.EndPoints;
using SafeMum.Application.Features.Users.ForgotPassword;
using SafeMum.Infrastructure.Configuration;


using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

// Get Heroku port or default
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Register services
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SafeMum API", Version = "v1" });
});

builder.Services.AddInfrastructure(); // Your custom DI extension

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ForgotPasswordRequest).Assembly);
});

// JWT Auth setup
var jwtKeys = builder.Configuration.GetSection("JwtSettings");
string authority = jwtKeys["Authority"];
string issuer = jwtKeys["Issuer"];
string secretKey = jwtKeys["secretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://qpmlnlojjsdnqohhhyth.supabase.co/auth/v1",
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("vQWXjdI1QHTzkq4D7h8Aagr43eqfT+1qux61soC4j6csCEIyVgZ/b3uhllsZ18W3NX5fESWxmY9FIuCyuFD5NA=="))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


// Heroku HTTPS redirection & headers
if (!app.Environment.IsDevelopment())
{
    // Replace current forwarded headers with:
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.Use(async (context, next) =>
    {
        if (context.Request.Headers["X-Forwarded-Proto"] != "https")
        {
            var withHttps = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            context.Response.Redirect(withHttps);
        }
        else
        {
            await next();
        }
    });
}

// Use HTTPS redirection
//app.UseHttpsRedirection();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeMum API V1");
    c.RoutePrefix = "swagger"; // So Swagger is at /swagger
});

// Routing
app.MapGet("/", () => Results.Json(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow
}));

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseAuthentication();
app.UseAuthorization();

// Your custom endpoint maps
app.MapUserEndpoints();
app.MapContentEndPoints();

app.Run();

