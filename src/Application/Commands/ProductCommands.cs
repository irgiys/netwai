using Application.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<ProductDto>;

public record UpdateProductCommand(Guid Id, UpdateProductDto Dto) : IRequest<ProductDto>;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

public record AdjustStockCommand(Guid Id, int Quantity) : IRequest<ProductDto>;