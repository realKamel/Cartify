using Cartify.Domain.Entities;
using Cartify.Services.Specifications;
using Cartify.Shared.DataTransferObjects.Category;

namespace Cartify.Services.Features.CategoryFeatures.Specification;

public class CategorySpecification : Specification<Category, int>
{
    public CategorySpecification(CategoriesQueryParameters query)
        : base
        (category => string.IsNullOrWhiteSpace(query.Keyword) || category.Name.ToLower().Contains(
            query.Keyword.ToLower()))
    {
        if (query.Page.HasValue)
        {
            ApplyPagination(query.Page.Value, query.Limit);
        }
    }
}