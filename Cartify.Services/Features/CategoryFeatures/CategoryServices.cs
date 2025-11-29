using Cartify.Domain.Entities;
using Cartify.Domain.Exceptions;
using Cartify.Domain.Interfaces;
using Cartify.Services.Abstractions;
using Cartify.Services.Features.CategoryFeatures.Specification;
using Cartify.Services.Helper;
using Cartify.Shared;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Category;

namespace Cartify.Services.Features.CategoryFeatures;

public class CategoryServices(IUnitOfWork unitOfWork) : ICategoryServices
{
	private IGenericRepository<Category, int> Repository => unitOfWork.GetOrCreateRepository<Category, int>();

	public async Task<PagedList<CategoryResponseDto>> GetAllCategoriesAsync(CategoriesQueryParameters? query,
		CancellationToken cancellationToken = default)
	{
		var result = await Repository
			.GetAllAsync(new CategorySpecification(query), cancellationToken: cancellationToken);

		var count = await Repository.CountAsync(new CategoryCountSpecification(query), cancellationToken);

		var dtoList = result.Select(b => b.ToCategoryDto()).ToList();

		var list = new PagedList<CategoryResponseDto>
		{
			Data = dtoList,
			Limit = Math.Min(query.Limit, count),
			Page = query.Page.Value,
			Total = count
		};
		return list;
	}

	public async Task<CategoryResponseDto> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		var result = await Repository.GetByIdAsync(id, cancellationToken);
		if (result is null)
		{
			throw new CategoryNotFoundException("Category not found");
		}

		var dto = result.ToCategoryDto();
		return dto;
	}

	public async Task<CategoryResponseDto> AddCategoryAsync(CreateOrUpdatedCategoryRequestDto categoryRequestDto,
		CancellationToken cancellationToken = default)
	{
		var entity = categoryRequestDto.ToEntity();
		var guidPath = Guid.NewGuid().ToString();
		Repository.Add(entity);
		entity.Image =
			await CoreModuleHelper
				.SaveCoverImageAndGeneratePathAsync(categoryRequestDto.Image,
				entity.Slug, guidPath, ProductCatalogImagesDirectories.CategoryImagesDirectory, cancellationToken);
		return entity.ToCategoryDto();
	}

	public async Task UpdateCategory(int id, CreateOrUpdatedCategoryRequestDto categoryRequest,
		CancellationToken cancellationToken = default)
	{
		var result = await Repository.GetByIdAsync(id, cancellationToken);

		if (result is null)
		{
			throw new CategoryNotFoundException("Category not found");
		}

		//update logic
		result.Name = categoryRequest.Name;
		CoreModuleHelper.RemoveImagesFromStorage(result.Image!);

		var Imagepath = Path.GetDirectoryName(result.Image!);

		result.Image = await CoreModuleHelper
			.SaveCoverImageAndGeneratePathAsync(categoryRequest.Image, result.Slug,
				Imagepath!,
				ProductCatalogImagesDirectories.CategoryImagesDirectory,
				cancellationToken);

		Repository.Update(result);
	}

	public async Task DeleteCategory(int id, CancellationToken cancellationToken = default)
	{
		var result = await Repository.GetByIdAsync(id, cancellationToken);

		if (result is null)
		{
			throw new CategoryNotFoundException("Category not found");
		}

		Repository.Remove(result);
	}
}