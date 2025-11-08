using Cartify.Services.Abstractions;
using Cartify.Services.Features.AuthFeatures;
using Cartify.Services.Features.BrandFeatures;
using Cartify.Services.Features.CategoryFeatures;
using Cartify.Services.Features.ProductFeatures;
using Cartify.Services.Helper;
using Microsoft.Extensions.Configuration;

namespace Cartify.Services;

using Microsoft.Extensions.DependencyInjection;

public static class AppServicesLayerRegistration
{
    public static void AddServiceLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductServices, ProductServices>();
        services.AddScoped<IBrandServices, BrandServices>();
        services.AddScoped<ICategoryServices, CategoryServices>();
        services.AddOptions<JwtConfigurationOptions>()
            .Bind(configuration.GetSection(nameof(JwtConfigurationOptions)));
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<IUserServices, UserServices>();
    }
}