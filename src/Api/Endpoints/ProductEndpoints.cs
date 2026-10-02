using Application.Commands;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (IMediator mediator, int page = 1, int pageSize = 10) =>
        {
            var query = new GetProductsQuery(page, pageSize);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new GetProductQuery(id);
            var result = await mediator.Send(query);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPost("/", async (IMediator mediator, [FromBody] CreateProductDto dto) =>
        {
            var command = new CreateProductCommand(dto);
            var result = await mediator.Send(command);
            return Results.Created($"/api/products/{result.Id}", result);
        });

        group.MapPut("/{id:guid}", async (IMediator mediator, Guid id, [FromBody] UpdateProductDto dto) =>
        {
            var command = new UpdateProductCommand(id, dto);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapDelete("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var command = new DeleteProductCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });

        group.MapPatch("/{id:guid}/stock", async (IMediator mediator, Guid id, [FromBody] AdjustStockDto dto) =>
        {
            var command = new AdjustStockCommand(id, dto.Quantity);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });
    }

    private record AdjustStockDto(int Quantity);
}