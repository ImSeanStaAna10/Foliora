using Microsoft.EntityFrameworkCore;
using Foliora.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Foliora API",
        Version = "v1",
        Description = "API for Foliora - Professional Portfolio & Career Vault"
    });
});

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<FolioraDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                      "Host=127.0.0.1;Port=5433;Database=foliora_dev;Username=postgres;Password=postgres"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foliora API v1");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at root (http://localhost:<port>/)
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", async (FolioraDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { status = "healthy", databaseConnected = canConnect, timestamp = DateTimeOffset.UtcNow });
});

app.Run();

