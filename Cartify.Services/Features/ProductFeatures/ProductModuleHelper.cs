using Cartify.Domain.Entities;
using Cartify.Services.Helper;
using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Services.Features.ProductFeatures;

public static class ProductModuleHelper
{
    public static ProductResponseDto ToProductResponseDto(this Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Slug = product.Slug,
            ImageCover = product.ImageCover,
            Sold = product.Sold,
            Price = product.Price,
            Quantity = product.Quantity,
            Images = product.Images,
            RatingsAverage = product.RatingsAverage,
            RatingsCount = product.RatingsQuantity,
            CreatedAtUtc = product.CreatedAtUtc,
            UpdatedAtUtc = product.UpdatedAtUtc,
            Category = new ProductCategoryResponseDto
            {
                Id = product.Category.Id,
                UpdatedAtUtc = product.Category.UpdatedAtUtc,
                CreatedAtUtc = product.Category.CreatedAtUtc,
                Image = product.Category.Image
            },
            Brand = new ProductBrandResponseDto
            {
                Id = product.Brand.Id,
                Name = product.Brand.Name,
                Slug = product.Brand.Slug,
                Image = product.Brand.Image,
                CreatedAtUtc = product.Brand.CreatedAtUtc,
                UpdatedAtUtc = product.Brand.UpdatedAtUtc,
            },
        };
    }

    public static Product ToEntity(this CreateOrUpdateProductRequestDto productDto, Guid userId)
    {
        return new Product
        {
            Title = productDto.Title,
            Slug = CoreModuleHelper.GenerateSlug(productDto.Title),
            Description = productDto.Description,
            Quantity = productDto.Quantity,
            Price = productDto.Price,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedBy = userId,
            ImageCover = null,
            Images = null,
            BrandId = productDto.BrandId,
            CategoryId = productDto.CategoryId,
        };
    }
}