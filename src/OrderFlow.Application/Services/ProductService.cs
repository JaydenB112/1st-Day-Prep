using OrderFlow.Application.Common;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Application.Services;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<ProductDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct = default);
}

public class ProductService(IProductRepository products, IUnitOfWork unitOfWork) : IProductService
{
    public const int MaxPageSize = 100;

    public async Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var result = await products.GetPagedAsync(page, pageSize, ct);
        return new PagedResult<ProductDto>(
            result.Items.Select(ToDto).ToList(), result.Page, result.PageSize, result.TotalCount);
    }

    public async Task<ProductDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var product = await products.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Product), id);
        return ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        var product = new Product { Name = request.Name, Price = request.Price, StockQuantity = request.StockQuantity };
        await products.AddAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return ToDto(product);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct = default)
    {
        var product = await products.GetByIdAsync(id, ct) ?? throw new NotFoundException(nameof(Product), id);

        product.Name = request.Name;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(ct);
        return ToDto(product);
    }

    private static ProductDto ToDto(Product p) => new(p.Id, p.Name, p.Price, p.StockQuantity, p.IsActive);
}
