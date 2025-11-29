using Cartify.Domain.Entities;
using Cartify.Services.Specifications;
using Cartify.Shared;
using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Services.Features.ProductFeatures.Specifications;

public class ProductCountSpecification(ProductQueryParameters productQuery)
    : Specification<Product, int>(p =>
        (string.IsNullOrWhiteSpace(productQuery.Keyword) || p.Title.ToLower().Contains(productQuery.Keyword.ToLower()))
        &&
        (!productQuery.Brand.HasValue || p.BrandId == productQuery.Brand.Value)
        &&
        (!productQuery.Category.HasValue || p.CategoryId == productQuery.Category.Value));