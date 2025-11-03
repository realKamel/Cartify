using Cartify.Domain.Entities;
using Cartify.Services.Helper;
using Cartify.Shared.DataTransferObjects.Category;

namespace Cartify.Services.Features.CategoryFeatures;

public static class CategoryHelpers
{
    public static CategoryResponseDto ToCategoryDto(this Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Image = category.Image,
            CreatedAtUtc = category.CreatedAtUtc,
            UpdatedAtUtc = category.UpdatedAtUtc,
        };
    }

    public static Category ToEntity(this CreateOrUpdatedCategoryRequestDto categoryRequestDto, Guid userId)
    {
        return new Category
        {
            Name = categoryRequestDto.Name,
            Image = null,
            Slug = CoreModuleHelper.GenerateSlug(categoryRequestDto.Name),
            CreatedAtUtc = DateTime.UtcNow,
            CreatedBy = userId,

            //Todo
            BrandCategories = [] //Just For Testing
        };
    }
}