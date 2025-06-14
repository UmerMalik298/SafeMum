

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SafeMum.API.EndPoints;
using SafeMum.Application.Features.Users.ForgotPassword;
using SafeMum.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = new ServiceCollection();
services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ForgotPasswordRequest).Assembly));




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
            //  IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey))
           IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("vQWXjdI1QHTzkq4D7h8Aagr43eqfT+1qux61soC4j6csCEIyVgZ/b3uhllsZ18W3NX5fESWxmY9FIuCyuFD5NA=="))



        };
    });

//builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

   
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapUserEndpoints();
app.MapContentEndPoints();
app.UseDefaultFiles();
app.UseStaticFiles();




app.Run();
