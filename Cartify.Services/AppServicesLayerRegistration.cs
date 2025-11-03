using Cartify.Services.Abstractions;
using Cartify.Services.Features.BrandFeatures;
using Cartify.Services.Features.CategoryFeatures;
using Cartify.Services.Features.ProductFeatures;

namespace Cartify.Services;

using Microsoft.Extensions.DependencyInjection;

public static class AppServicesLayerRegistration
{
    public static void AddServiceLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IProductServices, ProductServices>();
        services.AddScoped<IBrandServices, BrandServices>();
        services.AddScoped<ICategoryServices, CategoryServices>();
    }
}