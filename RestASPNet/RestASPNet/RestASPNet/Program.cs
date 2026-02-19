using RestASPNet.Configurations;
using RestASPNet.Services;
using RestASPNet.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLogging(); 

builder.Services.AddControllers();
builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddSingleton<MathService>();
builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
