using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Brand;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

/// <summary>
/// Provides CRUD operations for brand management
/// </summary>
/// <remarks>
/// This controller allows you to create, read, update, and delete brands in the system.
/// Administrative privileges are required for write operations.
/// </remarks>
public class BrandsController(IBrandServices services) : V1BaseController
{
	/// <summary>
	/// Retrieves a paginated list of brands with optional filtering and sorting
	/// </summary>
	/// <param name="queryParameters">Optional query parameters for filtering, sorting, and pagination</param>
	/// <param name="cancellationToken">Cancellation token to cancel the request</param>
	/// <returns>Paginated list of brands</returns>
	/// <response code="200">Returns the paginated list of brands</response>
	/// <response code="400">If the query parameters are invalid</response>
	/// <response code="500">If there was an internal server error</response>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<BrandResponseDto>), 200)]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	[ProducesResponseType(typeof(ProblemDetails), 500)]
	public async Task<ActionResult<PagedList<BrandResponseDto>>> GetBrands(
		[FromQuery] BrandQueryParameters? queryParameters,
		CancellationToken cancellationToken)
	{
		var result = await services.GetAllBrandsAsync(queryParameters, cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// Retrieves a specific brand by its unique identifier
	/// </summary>
	/// <param name="id">The brand ID (positive integer)</param>
	/// <param name="cancellationToken">Cancellation token to cancel the request</param>
	/// <returns>The requested brand details</returns>
	/// <response code="200">Returns the brand details</response>
	/// <response code="404">If the brand with the specified ID was not found</response>
	/// <response code="400">If the ID format is invalid</response>
	[HttpGet("{id:int}")]
	[ProducesResponseType(typeof(BrandResponseDto), 200)]
	[ProducesResponseType(typeof(ProblemDetails), 404)]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	public async Task<ActionResult<BrandResponseDto>> GetBrandById(
		[FromRoute] int id,
		CancellationToken cancellationToken)
	{
		var result = await services.GetBrandByIdAsync(id, cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// Creates a new brand in the system
	/// </summary>
	/// <param name="requestDto">Brand creation data</param>
	/// <param name="cancellationToken">Cancellation token to cancel the request</param>
	/// <returns>The newly created brand</returns>
	/// <response code="201">Brand created successfully</response>
	/// <response code="400">If the request data is invalid</response>
	/// <response code="401">If user is not authenticated</response>
	/// <response code="403">If user does not have Admin role</response>
	/// <response code="409">If a brand with the same name already exists</response>
	[HttpPost]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(typeof(BrandResponseDto), 201)]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	[ProducesResponseType(typeof(ProblemDetails), 401)]
	[ProducesResponseType(typeof(ProblemDetails), 403)]
	[ProducesResponseType(typeof(ProblemDetails), 409)]
	public async Task<ActionResult<BrandResponseDto>> CreateOrUpdateBrand(
		[FromBody] CreateOrUpdateBrandRequestDto requestDto,
		CancellationToken cancellationToken)
	{
		var result = await services.AddBrandAsync(requestDto, cancellationToken);
		return CreatedAtAction(nameof(GetBrandById), new { id = result.Id }, result);
	}

	/// <summary>
	/// Updates an existing brand
	/// </summary>
	/// <param name="id">The brand ID to update</param>
	/// <param name="requestDto">Updated brand data</param>
	/// <param name="cancellationToken">Cancellation token to cancel the request</param>
	/// <returns>Success status</returns>
	/// <response code="200">Brand updated successfully</response>
	/// <response code="400">If the request data is invalid</response>
	/// <response code="401">If user is not authenticated</response>
	/// <response code="403">If user does not have Admin role</response>
	/// <response code="404">If the brand with specified ID was not found</response>
	[HttpPut("{id:int}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(ProblemDetails), 400)]
	[ProducesResponseType(typeof(ProblemDetails), 401)]
	[ProducesResponseType(typeof(ProblemDetails), 403)]
	[ProducesResponseType(typeof(ProblemDetails), 404)]
	public async Task<ActionResult> UpdateBrandAsync(
		[FromRoute] int id,
		[FromBody] CreateOrUpdateBrandRequestDto requestDto,
		CancellationToken cancellationToken)
	{
		await services.UpdateBrand(id, requestDto, cancellationToken);
		return Ok();
	}

	/// <summary>
	/// Deletes a brand with the given Id
	/// </summary>
	/// <param name="id">The brand ID to delete</param>
	/// <param name="cancellationToken">Cancellation token to cancel the request</param>
	/// <returns>No content</returns>
	/// <response code="204">Brand deleted successfully</response>
	/// <response code="401">If user is not authenticated</response>
	/// <response code="403">If user does not have Admin role</response>
	/// <response code="404">If the brand with specified ID was not found</response>
	/// <response code="409">If the brand cannot be deleted due to existing references</response>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(204)]
	[ProducesResponseType(typeof(ProblemDetails), 401)]
	[ProducesResponseType(typeof(ProblemDetails), 403)]
	[ProducesResponseType(typeof(ProblemDetails), 404)]
	[ProducesResponseType(typeof(ProblemDetails), 409)]
	public async Task<ActionResult> DeleteBrand(
		[FromRoute] int id,
		CancellationToken cancellationToken)
	{
		await services.DeleteBrand(id, cancellationToken);
		return NoContent();
	}
}