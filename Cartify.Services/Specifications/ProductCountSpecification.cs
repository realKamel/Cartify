using Cartify.Domain.Entities;
using Cartify.Shared;

namespace Cartify.Services.Specifications;

public class ProductCountSpecification(QueryParameters query)
    : Specification<Product, int>(p =>
        (string.IsNullOrWhiteSpace(query.Keyword) || p.Title.ToLower().Contains(query.Keyword.ToLower()))
        &&
        (!query.Brand.HasValue || p.BrandId == query.Brand.Value)
        &&
        (!query.Category.HasValue || p.CategoryId == query.Category.Value));