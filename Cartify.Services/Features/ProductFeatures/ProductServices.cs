using Cartify.Domain.Entities;
using Cartify.Domain.Exceptions;
using Cartify.Domain.Interfaces;
using Cartify.Services.Abstractions;
using Cartify.Services.Features.ProductFeatures.Specifications;
using Cartify.Services.Helper;
using Cartify.Shared;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Product;
using Microsoft.Extensions.Logging;


namespace Cartify.Services.Features.ProductFeatures;

public class ProductServices(IUnitOfWork unitOfWork, ILogger<ProductServices> logger) : IProductServices
{
    private readonly IGenericRepository<Product, int> _productRepo = unitOfWork
        .GetOrCreateRepository<Product, int>();

    public async Task<PagedList<ProductResponseDto>> GetAllProductAsync(ProductQueryParameters query,
        CancellationToken cancellationToken)
    {
        var orderBy = query?.OrderBy;
        var orderByDesc = query?.OrderByDesc;
        if (orderBy.HasValue && orderByDesc.HasValue && orderByDesc.Value == orderBy.Value)
        {
            throw new CartItemNotFoundException("Can't Have the same field to sort ascending and descending");
        }

        var specification = new ProductSpecification(query!);

        var productRepoResult = await _productRepo
            .GetAllAsync(specification, cancellationToken: cancellationToken);

        var count = await _productRepo.CountAsync(new ProductCountSpecification(query!), cancellationToken);

        var result = productRepoResult.Select(p => p.ToProductResponseDto()).ToList();

        var list = new PagedList<ProductResponseDto>
        {
            Data = result,
            Page = query?.Page ?? 1,
            Limit = Math.Min(query!.Limit, count),
            Total = count
        };

        return list;
    }

    public async Task<ProductResponseDto> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _productRepo.GetByIdAsync(new ProductWithIdSpecification(id), cancellationToken);

        if (result is null)
        {
            throw new ProductNotFoundException(id);
        }

        return result.ToProductResponseDto();
    }

    public async Task<ProductResponseDto> AddProductAsync(CreateOrUpdateProductRequestDto productDto,
        CancellationToken cancellationToken)
    {
        var productEntity = productDto.ToEntity();

        _productRepo.Add(productEntity);

        var pathGuid = Guid.NewGuid().ToString();

        productEntity.ImageCover =
            await CoreModuleHelper.SaveCoverImageAndGeneratePathAsync(productDto.ImageCover,
                productEntity.Slug,
                pathGuid,
                ProductCatalogImagesDirectories.ProductImagesDirectory,
                cancellationToken);

        if (productDto.Images is not null && productDto.Images.Count > 0)
        {
            productEntity.Images = await CoreModuleHelper.SaveMultiImageAndGeneratePathAsync(productDto.Images,
                productEntity.Slug, pathGuid, ProductCatalogImagesDirectories.ProductImagesDirectory, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        productEntity.Brand = await unitOfWork
                                  .GetOrCreateRepository<Brand, int>()
                                  .GetByIdAsync(productDto.BrandId, cancellationToken)
                              ?? throw new Exception("Brand Repository is null");

        productEntity.Category = await unitOfWork
                                     .GetOrCreateRepository<Category, int>()
                                     .GetByIdAsync(productDto.CategoryId, cancellationToken)
                                 ?? throw new Exception("Category Repository is null");

        return productEntity.ToProductResponseDto();
    }

    public async Task UpdateProduct(int id, CreateOrUpdateProductRequestDto productDto,
        CancellationToken cancellationToken)
    {
        var item = await _productRepo.GetByIdAsync(id, cancellationToken);

        if (item is null)
        {
            throw new ProductNotFoundException();
        }


        var updatedProduct = productDto.ToEntity();

        var imagesPath = Path.GetDirectoryName(updatedProduct.ImageCover!);

        CoreModuleHelper.RemoveImagesFromStorage(updatedProduct.ImageCover!);

        updatedProduct.ImageCover =
            await CoreModuleHelper
                .SaveCoverImageAndGeneratePathAsync(
                    productDto.ImageCover,
                    updatedProduct.Slug,
                    imagesPath!,
                    ProductCatalogImagesDirectories.ProductImagesDirectory,
                    cancellationToken
                );


        if (productDto.Images?.Count > 0)
        {
            updatedProduct.Images =
                await CoreModuleHelper
                    .SaveMultiImageAndGeneratePathAsync(productDto.Images,
                        updatedProduct.Slug,
                        updatedProduct.ImageCover!,
                        ProductCatalogImagesDirectories.ProductImagesDirectory, cancellationToken);
        }

        _productRepo.Update(updatedProduct);
    }

    public async Task DeleteProduct(int id, CancellationToken cancellationToken)
    {
        var item = await _productRepo.GetByIdAsync(id, cancellationToken);

        if (item is null)
        {
            throw new ProductNotFoundException();
        }

        try
        {
            CoreModuleHelper.RemoveImagesFromStorage(item.ImageCover!);
        }
        catch (Exception e)
        {
            logger.LogError("Error Happened During Deleting Images: {message}", e.Message);
            throw;
        }

        _productRepo.Remove(item);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}