using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Brand;

namespace Cartify.Services.Abstractions;

public interface IBrandServices
{
    public Task<PagedList<BrandResponseDto>> GetAllBrandsAsync(BrandQueryParameters? query,
        CancellationToken cancellationToken = default);

    public Task<BrandResponseDto> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<BrandResponseDto> AddBrandAsync(CreateOrUpdateBrandRequestDto brandDto,
        CancellationToken cancellationToken = default);

    public Task UpdateBrand(int id, CreateOrUpdateBrandRequestDto brandDto,
        CancellationToken cancellationToken = default);

    public Task DeleteBrand(int id, CancellationToken cancellationToken = default);
}