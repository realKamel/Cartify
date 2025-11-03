using Cartify.Domain.Entities;
using Cartify.Services.Specifications;
using Cartify.Shared.DataTransferObjects.Category;

namespace Cartify.Services.Features.CategoryFeatures.Specification;

public class CategoryCountSpecification(CategoriesQueryParameters query)
    : Specification<Category, int>(category =>
        string.IsNullOrWhiteSpace(query.Keyword) || category.Name.ToLower().Contains(query.Keyword.ToLower()))
{
}