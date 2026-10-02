using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("Default") 
            ?? throw new InvalidOperationException("Connection string 'Default' not found");

        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(cs));
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}