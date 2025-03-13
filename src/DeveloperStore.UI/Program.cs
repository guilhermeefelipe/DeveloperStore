using DeveloperStore.Services.Services;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var appConfigFile = Environment.GetEnvironmentVariable("APP_CONFIG_FILE");
if (!string.IsNullOrEmpty(appConfigFile) && File.Exists(appConfigFile))
{
    builder.Configuration.AddJsonFile(appConfigFile);
}

builder.Host.UseSerilog((hostContext, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(hostContext.Configuration)
        .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.Converters.Add(new StringEnumConverter()));

builder.Services.AddDeveloperStoreServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeveloperStore", Version = "v1" });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Services.MigrateDatabaseAsync().Wait();

var supportedCultures = new[] { new CultureInfo("pt-BR") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
