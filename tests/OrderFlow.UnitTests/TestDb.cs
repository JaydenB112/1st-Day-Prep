using Microsoft.EntityFrameworkCore;
using OrderFlow.Infrastructure.Persistence;

namespace OrderFlow.UnitTests;

internal static class TestDb
{
    /// <summary>Fresh, isolated in-memory database per test.</summary>
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}
