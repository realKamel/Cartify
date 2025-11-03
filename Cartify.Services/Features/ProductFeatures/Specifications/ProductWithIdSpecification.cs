using Cartify.Domain.Entities;
using Cartify.Services.Specifications;

namespace Cartify.Services.Features.ProductFeatures.Specifications;

public class ProductWithIdSpecification : Specification<Product, int>
{
    public ProductWithIdSpecification(int id) : base(p => p.Id == id)
    {
        AddRelatedDataInclude(p => p.Category);
        AddRelatedDataInclude(p => p.Brand);
    }
}