using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Common;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Services;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    /// <summary>GET /api/products?page=1&amp;pageSize=10</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default) =>
        Ok(await productService.GetPagedAsync(page, pageSize, ct));

    /// <summary>GET /api/products/{id}</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct) =>
        Ok(await productService.GetByIdAsync(id, ct));

    /// <summary>POST /api/products</summary>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request, CancellationToken ct)
    {
        var created = await productService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>PUT /api/products/{id}</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductRequest request, CancellationToken ct) =>
        Ok(await productService.UpdateAsync(id, request, ct));
}
