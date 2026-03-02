using RestASPNet.Configurations;
using RestASPNet.Services;
using RestASPNet.Repositories;
using RestASPNet.Services.Impl;
using RestASPNet.Repositories.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLogging(); 

builder.Services.AddControllers().AddContentNegotiation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenAPIConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddRouterConfig();

builder.Services.AddCorsConfiguration(builder.Configuration);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddSingleton<MathService>();

builder.Services.AddScoped<IPersonServices, PersonServicesImplV1>();
builder.Services.AddScoped<PersonServicesImplV2>();

builder.Services.AddScoped<IBookServices, BookServicesImpl>();


builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// builder.Services.AddScoped<IPersonRepository, PersonRepository>();// Before generic repository
// builder.Services.AddScoped<IBookRepository, BookRepository>(); // Before generic repository

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseCorsConfiguration();

app.MapControllers();

app.UseSwaggerSpecification();
app.UseScalarConfig();

app.Run();
