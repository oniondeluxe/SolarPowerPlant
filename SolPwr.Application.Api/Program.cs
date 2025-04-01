using OnionDlx.SolPwr.Application.Services;
using OnionDlx.SolPwr.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add our services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddPersistence(connectionString);
builder.Services.AddBusinessLogic();
builder.Services.AddAuthServices(builder.Configuration, connectionString);

// Add the integration to some meterological forecast service
builder.Services.AddIntegrationExtensions(builder.Configuration.GetSection("MeteoService"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// The spinner for meteo data download
PlantOperationSpinner.Enabled = bool.Parse(builder.Configuration.GetSection("MeteoService")["Worker-Enabled"]);
builder.Services.AddHostedService<PlantOperationSpinner>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
