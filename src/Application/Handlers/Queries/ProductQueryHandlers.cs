using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using MediatR;

namespace Application.Handlers.Queries;

public sealed class GetProductHandler(IProductRepository repo) : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery query, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(query.Id, ct);
        return product is null ? null : ToDto(product);
    }

    private static ProductDto ToDto(Domain.Entities.Product p) => new(p.Id, p.Name, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt);
}

public sealed class GetProductsHandler(IProductRepository repo) : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var products = await repo.GetAllAsync(query.Page, query.PageSize, ct);
        return products.Select(ToDto).ToList();
    }

    private static ProductDto ToDto(Domain.Entities.Product p) => new(p.Id, p.Name, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt);
}