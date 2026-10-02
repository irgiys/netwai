using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Commands;

public sealed class CreateProductHandler(IProductRepository repo) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand cmd, CancellationToken ct)
    {
        var product = new Product(cmd.Dto.Name, cmd.Dto.Price, cmd.Dto.Stock);
        await repo.AddAsync(product, ct);
        return ToDto(product);
    }

    private static ProductDto ToDto(Product p) => new(p.Id, p.Name, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt);
}

public sealed class UpdateProductHandler(IProductRepository repo) : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand cmd, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(cmd.Id, ct) 
            ?? throw new KeyNotFoundException($"Product {cmd.Id} not found");
        
        product.Update(cmd.Dto.Name, cmd.Dto.Price, cmd.Dto.Stock);
        await repo.UpdateAsync(product, ct);
        return ToDto(product);
    }

    private static ProductDto ToDto(Product p) => new(p.Id, p.Name, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt);
}

public sealed class DeleteProductHandler(IProductRepository repo) : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand cmd, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(cmd.Id, ct) 
            ?? throw new KeyNotFoundException($"Product {cmd.Id} not found");
        
        await repo.DeleteAsync(product, ct);
        return Unit.Value;
    }
}

public sealed class AdjustStockHandler(IProductRepository repo) : IRequestHandler<AdjustStockCommand, ProductDto>
{
    public async Task<ProductDto> Handle(AdjustStockCommand cmd, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(cmd.Id, ct) 
            ?? throw new KeyNotFoundException($"Product {cmd.Id} not found");
        
        product.AdjustStock(cmd.Quantity);
        await repo.UpdateAsync(product, ct);
        return ToDto(product);
    }

    private static ProductDto ToDto(Product p) => new(p.Id, p.Name, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt);
}