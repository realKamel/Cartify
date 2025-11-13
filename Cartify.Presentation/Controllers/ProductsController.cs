using Cartify.Services.Abstractions;
using Cartify.Shared;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;


/// <summary>
/// Provides CRUD operations for Product management.
/// </summary>
/// <param name="services">The product service used to perform business operations related to products. Cannot be null.</param>
public class ProductsController(IProductServices services) : V1BaseController
{

	/// <summary>
	/// Retrieves a paginated list of products that match the specified query parameters.
	/// </summary>
	/// <remarks>Returns a 200 OK response with the paged product list if successful, or a 400 Bad Request with
	/// problem details if the query parameters are invalid.</remarks>
	/// <param name="query">An object containing filtering, sorting, and pagination options for the product list. If null, all products are
	/// returned without filtering.</param>
	/// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
	/// <returns>An <see cref="ActionResult{T}"/> containing a paged list of product data transfer objects if the request is
	/// successful; otherwise, a problem details response indicating why the request failed.</returns>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<ProductResponseDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<PagedList<ProductResponseDto>>> GetAllProductAsync(
			[FromQuery] ProductQueryParameters? query,
			CancellationToken cancellationToken)
	{
		var result = await services.GetAllProductAsync(query, cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// Retrieves items with the given Id.
	/// </summary>
	/// <param name="Id">The page number (optional).</param>
	/// <returns>The product that match the Id </returns>
	[HttpGet("{id:int}")]
	[ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ProductResponseDto>> GetProductWithId(int id,
		CancellationToken cancellationToken)
	{
		var result = await services.GetProductByIdAsync(id, cancellationToken);
		return Ok(result);
	}


	/// <summary>
	/// Creates a new product using the specified details and returns the created product information.
	/// </summary>
	/// <remarks>Requires the caller to be authorized with the 'Admin' role. The request must be sent as
	/// 'multipart/form-data'. Returns 400 Bad Request if the product data is invalid, 401 Unauthorized if the user is not
	/// authenticated, and 403 Forbidden if the user does not have the required role.</remarks>
	/// <param name="product">The product data to create, including name, description, price, and any associated files. Must not be null.</param>
	/// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
	/// <returns>An ActionResult containing the created product information if successful. Returns a 201 Created response with the
	/// product data, or an error response if the request is invalid or unauthorized.</returns>

	[HttpPost]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[Consumes("multipart/form-data")]
	public async Task<ActionResult<ProductResponseDto>> CreateProductAsync(
		[FromForm] CreateOrUpdateProductRequestDto product,
		CancellationToken cancellationToken)
	{
		var result = await services.AddProductAsync(product,
			cancellationToken);
		return CreatedAtAction(nameof(GetProductWithId), new { id = result.Id }, result);
	}




	/// <summary>
	/// Updates the details of an existing product with the specified identifier.
	/// </summary>
	/// <remarks>Requires the caller to have the 'Admin' role. Returns a 404 Not Found response if the product does
	/// not exist, or a 400 Bad Request response if the input is invalid.</remarks>
	/// <param name="id">The unique identifier of the product to update.</param>
	/// <param name="productRequestDto">An object containing the updated product information. Cannot be null.</param>
	/// <param name="cancellationToken">A token that can be used to cancel the update operation.</param>
	/// <returns>A result indicating the outcome of the update operation. Returns a 204 No Content response if the update is
	/// successful.</returns>
	[HttpPut("{id:int}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> UpdateProductAsync(int id,
		CreateOrUpdateProductRequestDto productRequestDto, CancellationToken cancellationToken)
	{
		await services.UpdateProduct(id, productRequestDto, cancellationToken);
		return NoContent();
	}






	/// <summary>
	/// Deletes the product with the specified identifier.
	/// </summary>
	/// <remarks>Requires the caller to have the 'Admin' role. Returns 401 Unauthorized if the user is not
	/// authenticated, or 403 Forbidden if the user does not have sufficient permissions.</remarks>
	/// <param name="id">The unique identifier of the product to delete.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
	/// <returns>A result indicating the outcome of the delete operation. Returns a 204 No Content response if the product is
	/// successfully deleted; returns a 404 Not Found response if the product does not exist.</returns>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
	{
		await services.DeleteProduct(id, cancellationToken);
		return NoContent();
	}
}