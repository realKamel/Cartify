using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Category;

namespace Cartify.Services.Abstractions;

public interface ICategoryServices
{
    public Task<PagedList<CategoryResponseDto>> GetAllCategoriesAsync(CategoriesQueryParameters? query,
        CancellationToken cancellationToken = default);

    public Task<CategoryResponseDto> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<CategoryResponseDto> AddCategoryAsync(CreateOrUpdatedCategoryRequestDto categoryRequestDto,
        CancellationToken cancellationToken = default);

    public Task UpdateCategory(int id, CreateOrUpdatedCategoryRequestDto categoryRequest,
        CancellationToken cancellationToken = default);

    public Task DeleteCategory(int id, CancellationToken cancellationToken = default);
}