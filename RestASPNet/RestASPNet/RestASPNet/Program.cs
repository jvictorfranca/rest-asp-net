using RestASPNet.Configurations;
using RestASPNet.Services;
using RestASPNet.Repositories;
using RestASPNet.Services.Impl;
using RestASPNet.Repositories.Impl;
using RestASPNet.Hypermedia.Filters;
using RestASPNet.Files.Importers.Factory;
using RestASPNet.Files.Exporters.Factory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddSerilogLogging(); 

builder.Services.AddControllers(
        options => {
            options.Filters.Add<HypermediaFilter>();
            }
    ).
    AddContentNegotiation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenAPIConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddRouterConfig();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddHATEOASConfiguration();

builder.Services.AddEmailConfiguration(builder.Configuration);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddSingleton<MathService>();

builder.Services.AddScoped<IPersonServices, PersonServicesImplV1>();
builder.Services.AddScoped<PersonServicesImplV2>();
builder.Services.AddScoped<CSVFileImporter>();
builder.Services.AddScoped<XlsxFileImporter>();
builder.Services.AddScoped<FileImporterFactory>();


builder.Services.AddScoped<CsvExporter>();
builder.Services.AddScoped<XlsxExporter>();
builder.Services.AddScoped<FileExporterFactory>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IFileServices, FileServicesImpl>();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IBookServices, BookServicesImpl>();


builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// builder.Services.AddScoped<IPersonRepository, PersonRepository>();// Before generic repository
// builder.Services.AddScoped<IBookRepository, BookRepository>(); // Before generic repository

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
//app.UseCorsConfiguration();
app.UseCorsConfiguration( builder.Configuration);

app.MapControllers();
app.UseHATEOASRoutes();

app.UseSwaggerSpecification();
app.UseScalarConfig();

app.Run();
