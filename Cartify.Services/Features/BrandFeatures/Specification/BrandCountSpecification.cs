using Cartify.Domain.Entities;
using Cartify.Services.Specifications;
using Cartify.Shared.DataTransferObjects.Brand;

namespace Cartify.Services.Features.BrandFeatures.Specification;

public class BrandCountSpecification(BrandQueryParameters query)
    : Specification<Brand, int>(brand =>
        string.IsNullOrWhiteSpace(query.Keyword) || brand.Name.ToLower().Contains(query.Keyword.ToLower()))
{
}