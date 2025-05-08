using AzureCognitiveIntegration.Core.Extensions;
using AzureCognitiveIntegration.Features.DocumentAnalysis.Services;
using AzureCognitiveIntegration.Features.Vision.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information("Starting up Environment: {Environment}", environment);

try
{
    // Add services to the container.
    var configuration = builder.Configuration;
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.CustomAddSwaggerGen(configuration);
    builder.AddLoggingService();

    builder.Services.AddControllers();
    builder.Services.AddScoped<IDocumentAnalysisService, DocumentAnalysisService>();
    builder.Services.AddScoped<IAzureServiceWorker, AzureServiceWorker>();
    builder.Services.AddScoped<IVisionService, VisionService>();

    
    
    var app = builder.Build();
    // Configure the HTTP request pipeline.
  
    app.CustomUseSwagger(configuration);
    app.UseHttpsRedirection();
    app.MapControllers();
    
    Log.Information("The app started with environment: {Environment}", environment);
    app.Run();
}
catch (Exception ex) when(ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore")
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("The app is shutting down");
    Log.CloseAndFlush();
}












