

using Microsoft.IdentityModel.Tokens;
using SafeMum.API.EndPoints;
using SafeMum.Application.Features.Users.ForgotPassword;
using SafeMum.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authority;
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

   
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapUserEndpoints();
app.UseDefaultFiles();
app.UseStaticFiles();




app.Run();
