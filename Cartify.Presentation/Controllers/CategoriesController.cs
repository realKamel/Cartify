using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Brand;
using Cartify.Shared.DataTransferObjects.Category;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

public class CategoriesController(ICategoryServices services) : V1BaseController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<BrandResponseDto>>>
        GetBrands(
            [FromQuery] CategoriesQueryParameters? queryParameters,
            CancellationToken cancellationToken)
    {
        var result = await services
            .GetAllCategoriesAsync(queryParameters, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDto>> GetBrandById(int id,
        CancellationToken cancellationToken)
    {
        var result = await services.GetCategoryByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> CreateOrUpdateBrand(
        CreateOrUpdatedCategoryRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var result = await services.AddCategoryAsync(requestDto, cancellationToken);
        return CreatedAtAction(nameof(GetBrandById),
            new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBrandAsync(int id, CreateOrUpdatedCategoryRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.UpdateCategory(id, requestDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBrand(int id, CancellationToken cancellationToken)
    {
        await services.DeleteCategory(id, cancellationToken);
        return NoContent();
    }
}