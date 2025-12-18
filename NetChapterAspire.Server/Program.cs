using Microsoft.EntityFrameworkCore;
using NetChapterAspire.Server.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddAzureBlobServiceClient("Blobs");

// Add Azure Service Bus
builder.AddAzureServiceBusClient("ServiceBus");

// Add SQL Server with EF Core
builder.AddSqlServerDbContext<ApplicationDbContext>("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Apply migrations automatically on startup
using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

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
