using Cartify.Shared;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Services.Abstractions;

public interface IProductServices
{
    public Task<PagedList<ProductResponseDto>> GetAllProductAsync(ProductQueryParameters query,
        CancellationToken cancellationToken = default);

    public Task<ProductResponseDto> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<ProductResponseDto> AddProductAsync(CreateOrUpdateProductRequestDto product,
        CancellationToken cancellationToken = default);

    public Task UpdateProduct(int id, CreateOrUpdateProductRequestDto product,
        CancellationToken cancellationToken = default);

    public Task DeleteProduct(int id, CancellationToken cancellationToken = default);
}