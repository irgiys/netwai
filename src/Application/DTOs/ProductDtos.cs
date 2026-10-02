namespace Application.DTOs;

public record ProductDto(Guid Id, string Name, decimal Price, int Stock, DateTime CreatedAt, DateTime? UpdatedAt);

public record CreateProductDto(string Name, decimal Price, int Stock);

public record UpdateProductDto(string Name, decimal Price, int Stock);