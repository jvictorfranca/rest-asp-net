using RestASPNet.Configurations;
using RestASPNet.Services;
using RestASPNet.Repositories;
using RestASPNet.Services.Impl;
using RestASPNet.Repositories.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLogging(); 

builder.Services.AddControllers();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddSingleton<MathService>();

builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped<IBookServices, BookServicesImpl>();
builder.Services.AddScoped<IBookRepository, BookRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
