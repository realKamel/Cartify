using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Brand;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

public class BrandsController(IBrandServices services) : V1BaseController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<BrandResponseDto>>>
        GetBrands(
            [FromQuery] BrandQueryParameters? queryParameters,
            CancellationToken cancellationToken)
    {
        var result = await services.GetAllBrandsAsync(queryParameters, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BrandResponseDto>> GetBrandById(int id,
        CancellationToken cancellationToken)
    {
        var result = await services.GetBrandByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BrandResponseDto>> CreateOrUpdateBrand(CreateOrUpdateBrandRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var result = await services.AddBrandAsync(requestDto, cancellationToken);
        return CreatedAtAction(nameof(GetBrandById),
            new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBrandAsync(int id, CreateOrUpdateBrandRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await services.UpdateBrand(id, requestDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBrand(int id, CancellationToken cancellationToken)
    {
        await services.DeleteBrand(id, cancellationToken);
        return NoContent();
    }
}