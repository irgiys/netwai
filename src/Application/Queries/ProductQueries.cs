using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetProductQuery(Guid Id) : IRequest<ProductDto?>;

public record GetProductsQuery(int Page, int PageSize) : IRequest<IReadOnlyList<ProductDto>>;