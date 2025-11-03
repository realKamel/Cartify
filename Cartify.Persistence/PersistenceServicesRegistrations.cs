using Cartify.Domain.Interfaces;
using Cartify.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cartify.Persistence;

public static class PersistenceServicesRegistrations
{
    public static void AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //dbContexts configuration
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}