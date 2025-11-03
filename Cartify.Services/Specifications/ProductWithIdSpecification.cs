using System.Linq.Expressions;
using Cartify.Domain.Entities;

namespace Cartify.Services.Specifications;

public class ProductWithIdSpecification : Specification<Product, int>
{
    public ProductWithIdSpecification(int id) : base(p => p.Id == id)
    {
        AddRelatedDataInclude(p => p.Category);
        AddRelatedDataInclude(p => p.Brand);
    }
}