using Api.Middleware;
using Serilog;
using Api.Endpoints;
using Application;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapProductEndpoints();

app.Run();
