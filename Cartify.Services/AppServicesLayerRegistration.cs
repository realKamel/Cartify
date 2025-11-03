using Cartify.Services.Abstractions;
using Cartify.Services.Features.ProductModule;

namespace Cartify.Services;

using Microsoft.Extensions.DependencyInjection;

public static class AppServicesLayerRegistration
{
    public static void AddServiceLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IProductServices, ProductServices>();
    }
}