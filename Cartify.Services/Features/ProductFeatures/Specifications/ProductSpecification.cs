using System.Linq.Expressions;
using Cartify.Domain.Entities;
using Cartify.Services.Specifications;
using Cartify.Shared;
using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Services.Features.ProductFeatures.Specifications;

public class ProductSpecification : Specification<Product, int>
{
    public ProductSpecification(ProductQueryParameters query)
        : base(p =>
            (string.IsNullOrWhiteSpace(query.Keyword) || p.Title.ToLower().Contains(query.Keyword))
            &&
            (!query.Brand.HasValue || p.BrandId == query.Brand.Value)
            &&
            (!query.Category.HasValue || p.CategoryId == query.Category.Value))
    {
        AddRelatedDataInclude(product => product.Brand);
        AddRelatedDataInclude(product => product.Category);

        if (query.OrderBy is not null)
        {
            Expression<Func<Product, object>> order = query.OrderBy switch
            {
                OrderByEnum.Price => product => product.Price,
                OrderByEnum.Title => product => product.Title,
                OrderByEnum.Sold => product => product.Sold,
                OrderByEnum.CreationTime => product => product.CreatedBy,
                OrderByEnum.UpdateTime => product => product.UpdatedAtUtc!,
                OrderByEnum.RatingsAverage => product => product.RatingsAverage,
                OrderByEnum.RatingsQuantity => product => product.RatingsQuantity,
                OrderByEnum.Quantity => product => product.Quantity,
                _ => throw new ArgumentOutOfRangeException($"{query.OrderBy.ToString()} is invalid")
            };
            AddOrderBy(order);
        }

        if (query.OrderByDesc is not null)
        {
            Expression<Func<Product, object>> order = query.OrderByDesc switch
            {
                OrderByEnum.Price => product => product.Price,
                OrderByEnum.Title => product => product.Title,
                OrderByEnum.Sold => product => product.Sold,
                OrderByEnum.CreationTime => product => product.CreatedBy,
                OrderByEnum.UpdateTime => product => product.UpdatedAtUtc!,
                OrderByEnum.RatingsAverage => product => product.RatingsAverage,
                OrderByEnum.RatingsQuantity => product => product.RatingsQuantity,
                OrderByEnum.Quantity => product => product.Quantity,
                _ => throw new ArgumentOutOfRangeException($"{query.OrderByDesc.ToString()} is invalid")
            };
            AddOrderByDesc(order);
        }

        if (query is { Page: not null })
        {
            ApplyPagination(query.Page.Value, query.Limit);
        }
    }
}