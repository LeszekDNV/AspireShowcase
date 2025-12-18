using Microsoft.EntityFrameworkCore;
using NetChapterAspire.Server.Data;
using NetChapterAspire.Server.Middleware;
using NetChapterAspire.Server.Services;
using NetChapterAspire.Server.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddAzureBlobServiceClient("Blobs");
builder.AddAzureServiceBusClient("ServiceBus");
builder.AddSqlServerDbContext<ApplicationDbContext>("DefaultConnection");

// Register application services
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IServiceBusService, ServiceBusService>();

// Add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Apply migrations automatically on startup
using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
