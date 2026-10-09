namespace OrderFlow.Application.Dtos;

public record ProductDto(int Id, string Name, decimal Price, int StockQuantity, bool IsActive);

public record CreateProductRequest(string Name, decimal Price, int StockQuantity);

public record UpdateProductRequest(string Name, decimal Price, int StockQuantity, bool IsActive);
