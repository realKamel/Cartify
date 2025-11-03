using Cartify.Domain.Entities;
using Cartify.Domain.Exceptions;
using Cartify.Domain.Interfaces;
using Cartify.Services.Abstractions;
using Cartify.Services.Features.BrandFeatures.Specification;
using Cartify.Services.Helper;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Brand;

namespace Cartify.Services.Features.BrandFeatures;

public class BrandServices(IUnitOfWork unitOfWork) : IBrandServices
{
    private IGenericRepository<Brand, int> Repository => unitOfWork.GetOrCreateRepository<Brand, int>();

    public async Task<PagedList<BrandResponseDto>> GetAllBrandsAsync(BrandQueryParameters? query,
        CancellationToken cancellationToken)
    {
        var result = await Repository
            .GetAllAsync(new BrandSpecification(query), cancellationToken: cancellationToken);

        var count = await Repository.CountAsync(new BrandCountSpecification(query), cancellationToken);

        var brandDtoList = result.Select(b => b.ToBrandDto()).ToList();

        var list = new PagedList<BrandResponseDto>
        {
            Data = brandDtoList,
            Limit = Math.Min(query.Limit, count),
            Page = query.Page.Value,
            Total = count
        };
        return list;
    }

    public async Task<BrandResponseDto> GetBrandByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await Repository.GetByIdAsync(id, cancellationToken);
        if (result is null)
        {
            throw new BrandNotFoundException("Brand not found");
        }

        var brandDto = result.ToBrandDto();
        return brandDto;
    }

    public async Task<BrandResponseDto> AddBrandAsync(CreateOrUpdateBrandRequestDto brandDto,
        CancellationToken cancellationToken = default)
    {
        var entity = brandDto.ToEntity(Guid.NewGuid());
        var guidPath = Guid.NewGuid().ToString();
        Repository.Add(entity);
        entity.Image =
            await CoreModuleHelper
                .SaveCoverImageAndGeneratePathAsync(brandDto.Image, entity.Slug, guidPath, "brands",
                    cancellationToken);
        return entity.ToBrandDto();
    }

    public async Task UpdateBrand(int id, CreateOrUpdateBrandRequestDto brandDto,
        CancellationToken cancellationToken = default)
    {
        var result = await Repository.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            throw new BrandNotFoundException("Brand not found");
        }

        //update logic
        result.Name = brandDto.Name;
        CoreModuleHelper.RemoveImagesFromStorage(result.Image!);

        var Imagepath = Path.GetDirectoryName(result.Image!);

        result.Image = await CoreModuleHelper
            .SaveCoverImageAndGeneratePathAsync(brandDto.Image, result.Slug,
                Imagepath!,
                "brands",
                cancellationToken);
        result.UpdatedAtUtc = DateTime.UtcNow;
        result.UpdatedBy = Guid.NewGuid();

        Repository.Update(result);
    }

    public async Task DeleteBrand(int id, CancellationToken cancellationToken = default)
    {
        var result = await Repository.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            throw new BrandNotFoundException("Brand not found");
        }

        Repository.Remove(result);
    }
}