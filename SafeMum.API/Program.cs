

using SafeMum.API.EndPoints;
using SafeMum.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

   
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapUserEndpoints();
app.UseDefaultFiles();
app.UseStaticFiles();




app.Run();
