using System.Text.Json.Serialization;
using Baseline;
using deftq.Akkordplus.WebApplication;
using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Integration.Outbox.Configuration;
using deftq.BuildingBlocks.Serialization;
using deftq.BuildingBlocks.Validation.Webapi.Middelware;
using deftq.Catalog.Infrastructure.Configuration;
using deftq.Pieceworks.Infrastructure.Configuration;
using deftq.UserAccess.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new SystemTextDateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new SystemTextDateTimeOffsetJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAdB2C");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string" });
});
builder.Services.UseBuildingBlocks();
builder.Services.UseUserAccess();
builder.Services.UsePieceworks(builder.Environment, builder.Configuration);
builder.Services.UseCatalog();
builder.Services.AddMarten(builder.Configuration, true);
builder.Services.AddOutbox();
builder.Services.AddSpaStaticFiles(conf => conf.RootPath = "client-app/build");
builder.Services.Configure<FormOptions>(opt =>
{
    var maxUploadFileSizeMB = builder.Configuration.GetSection("Environment").GetValue<int>("MaxUploadFileSizeMB");
    if (maxUploadFileSizeMB > 0)
    {
        opt.MultipartBodyLengthLimit = maxUploadFileSizeMB * 1024 * 1024;
    }
});

var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Filter.With(new PingEndpointLogFilter())
    .WriteTo.Console();

// If not development environment, then enable logging to ApplicationInsights
if (!builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING") ?? "";
    if (connectionString.IsNotEmpty())
    {
        loggerConfig.WriteTo.ApplicationInsights(connectionString, new EventTelemetryConverter(), LogEventLevel.Information);
    }
}

var logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog(logger);
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>(Options.Create(new ExceptionHandlerOption { ShowCallStackInHttpResponse = true }));
app.UseMiddleware<RequestLogContextMiddleware>();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSerilogRequestLogging();

app.UseCors(
    opt =>
    {
        opt.AllowAnyOrigin();
        opt.AllowAnyHeader();
        opt.AllowAnyMethod();
    }
);

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "client-app";
});

app.Run();
