using Cartify.Services.Abstractions;
using Cartify.Shared;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Product;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

public class ProductsController(IProductServices services) : V1BaseController
{
    [HttpGet]
    public async Task<ActionResult<PagedList<ProductResponseDto>>> GetAllProductAsync(
        [FromQuery] ProductQueryParameters? query,
        CancellationToken cancellationToken)
    {
        var result = await services.GetAllProductAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetProductWithId(int id,
        CancellationToken cancellationToken)
    {
        var result = await services.GetProductByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateProductAsync(
        [FromForm] CreateOrUpdateProductRequestDto product,
        CancellationToken cancellationToken)
    {
        var result = await services.AddProductAsync(product,
            cancellationToken);
        return CreatedAtAction(nameof(GetProductWithId), new { id = result.Id }, result);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProductAsync(int id,
        CreateOrUpdateProductRequestDto productRequestDto, CancellationToken cancellationToken)
    {
        await services.UpdateProduct(id, productRequestDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        await services.DeleteProduct(id, cancellationToken);
        return NoContent();
    }
}