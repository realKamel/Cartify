using Cartify.Domain.Entities;
using Cartify.Services.Specifications;
using Cartify.Shared.DataTransferObjects.Brand;

namespace Cartify.Services.Features.BrandFeatures.Specification;

public class BrandSpecification : Specification<Brand, int>

{
    public BrandSpecification(BrandQueryParameters query) :
        base(brand => string.IsNullOrWhiteSpace(query.Keyword) || brand.Name.ToLower().Contains(
            query.Keyword.ToLower()))
    {
        if (query.Page.HasValue)
        {
            ApplyPagination(query.Page.Value, query.Limit);
        }
    }
}