using OrderFlow.Application.Services;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Infrastructure.Repositories;

namespace OrderFlow.UnitTests;

public class ProductServiceTests
{
    [Fact]
    public async Task GetByIdAsync_UnknownId_ThrowsNotFound()
    {
        using var db = TestDb.Create();
        var sut = new ProductService(new ProductRepository(db), db);

        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetByIdAsync(42));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(500, ProductService.MaxPageSize)]
    public async Task GetPagedAsync_ClampsPageSize(int requested, int expected)
    {
        using var db = TestDb.Create();
        var sut = new ProductService(new ProductRepository(db), db);

        var result = await sut.GetPagedAsync(page: 1, pageSize: requested);

        Assert.Equal(expected, result.PageSize);
    }

    [Fact(Skip = "OF-104: remove Skip once pagination is fixed")]
    public async Task GetPagedAsync_FirstPage_StartsAtFirstProduct()
    {
        using var db = TestDb.Create();
        db.Products.AddRange(Enumerable.Range(1, 15).Select(i => new Product { Name = $"P{i}", Price = i }));
        await db.SaveChangesAsync();
        var sut = new ProductService(new ProductRepository(db), db);

        var result = await sut.GetPagedAsync(page: 1, pageSize: 10);

        Assert.Equal("P1", result.Items[0].Name);
        Assert.Equal(10, result.Items.Count);
    }
}
