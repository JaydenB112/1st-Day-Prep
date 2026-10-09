using System.Text.Json.Serialization;
using OrderFlow.Api.Middleware;
using OrderFlow.Application;
using OrderFlow.Infrastructure;
using OrderFlow.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ---- Services (the DI container) -------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ---- Database bootstrap (dev only) -------------------------------------------
// TODO (OF-115): replace EnsureCreated with EF Core migrations.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}

// ---- HTTP pipeline (middleware runs top to bottom on the way in) -----------
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // GET /openapi/v1.json
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// A minimal API endpoint, to show the other routing style you'll see in .NET codebases.
app.MapGet("/api/version", () => Results.Ok(new { service = "OrderFlow", version = "1.4.2" }))
   .WithName("GetVersion");

app.Run();

// Lets integration tests use WebApplicationFactory<Program>.
public partial class Program;
