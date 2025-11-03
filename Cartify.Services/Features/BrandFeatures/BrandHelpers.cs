using Cartify.Domain.Entities;
using Cartify.Services.Helper;
using Cartify.Shared.DataTransferObjects.Brand;

namespace Cartify.Services.Features.BrandFeatures;

public static class BrandHelpers
{
    public static BrandResponseDto ToBrandDto(this Brand brand)
    {
        return new BrandResponseDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Slug = brand.Slug,
            Image = brand.Image,
            CreatedAtUtc = brand.CreatedAtUtc,
            UpdatedAtUtc = brand.UpdatedAtUtc,
        };
    }

    public static Brand ToEntity(this CreateOrUpdateBrandRequestDto brandDto, Guid userId)
    {
        return new Brand
        {
            Name = brandDto.Name,
            Image = null,
            Slug = CoreModuleHelper.GenerateSlug(brandDto.Name),
            CreatedAtUtc = DateTime.UtcNow,
            CreatedBy = userId,
            
            //Todo
            BrandCategories = [] //Just For Testing
        };
    }
}