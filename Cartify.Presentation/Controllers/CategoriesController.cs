using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects;
using Cartify.Shared.DataTransferObjects.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

/// <summary>
/// Controller for managing product categories
/// </summary>
/// <remarks>
/// Provides endpoints for retrieving, creating, updating, and deleting categories.
/// Administrative operations require 'Admin' role authorization.
/// </remarks>
public class CategoriesController(ICategoryServices services) : V1BaseController
{
	/// <summary>
	/// Retrieves a paginated list of categories
	/// </summary>
	/// <param name="queryParameters">Optional query parameters for filtering, sorting, and pagination</param>
	/// <param name="cancellationToken">Cancellation token for async operation</param>
	/// <returns>Paginated list of category records</returns>
	/// <response code="200">Returns the paginated list of categories</response>
	/// <response code="400">If the query parameters are invalid</response>
	/// <response code="500">If an internal server error occurs</response>
	[HttpGet]
	[ProducesResponseType<PagedList<CategoryResponseDto>>(StatusCodes.Status200OK)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<PagedList<CategoryResponseDto>>>
		GetCategories(
			[FromQuery] CategoriesQueryParameters? queryParameters,
			CancellationToken cancellationToken)
	{
		var result = await services
			.GetAllCategoriesAsync(queryParameters, cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// Retrieves a specific category by its unique identifier
	/// </summary>
	/// <param name="id">The unique identifier of the category</param>
	/// <param name="cancellationToken">Cancellation token for async operation</param>
	/// <returns>The requested category details</returns>
	/// <response code="200">Returns the category details</response>
	/// <response code="404">If the category with the specified ID is not found</response>
	/// <response code="500">If an internal server error occurs</response>
	[HttpGet("{id:int}")]
	[ProducesResponseType<CategoryResponseDto>(StatusCodes.Status200OK)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id,
		CancellationToken cancellationToken)
	{
		var result = await services.GetCategoryByIdAsync(id, cancellationToken);
		return Ok(result);
	}

	/// <summary>
	/// Creates a new category or updates an existing one
	/// </summary>
	/// <param name="requestDto">Category creation or update data</param>
	/// <param name="cancellationToken">Cancellation token for async operation</param>
	/// <returns>The created or updated category details</returns>
	/// <response code="201">Returns the created/updated category with location header</response>
	/// <response code="400">If the request data is invalid</response>
	/// <response code="401">If user is not authenticated</response>
	/// <response code="403">If user does not have Admin role</response>
	/// <response code="409">If a category with the same name already exists</response>
	/// <response code="500">If an internal server error occurs</response>
	[HttpPost]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType<CategoryResponseDto>(StatusCodes.Status201Created)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<CategoryResponseDto>> CreateOrUpdateCategory(
		CreateOrUpdatedCategoryRequestDto requestDto,
		CancellationToken cancellationToken)
	{
		var result = await services.AddCategoryAsync(requestDto, cancellationToken);
		return CreatedAtAction(nameof(GetCategoryById),
			new { id = result.Id }, result);
	}

	/// <summary>
	/// Updates an existing category
	/// </summary>
	/// <param name="id">The unique identifier of the category to update</param>
	/// <param name="requestDto">Category update data</param>
	/// <param name="cancellationToken">Cancellation token for async operation</param>
	/// <returns>Success status</returns>
	/// <response code="200">If the category was updated successfully</response>
	/// <response code="400">If the request data is invalid</response>
	/// <response code="401">If user is not authenticated</response>
	/// <response code="403">If user does not have Admin role</response>
	/// <response code="404">If the category with the specified ID is not found</response>
	/// <response code="409">If a category with the same name already exists</response>
	/// <response code="500">If an internal server error occurs</response>
	[HttpPut("{id:int}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> UpdateCategoryAsync(int id, [FromForm] CreateOrUpdatedCategoryRequestDto requestDto,
		CancellationToken cancellationToken)
	{
		await services.UpdateCategory(id, requestDto, cancellationToken);
		return NoContent();
	}

	/// <summary>
	/// Deletes a specific category
	/// </summary>
	/// <param name="id">The unique identifier of the category to delete</param>
	/// <param name="cancellationToken">Cancellation token for async operation</param>
	/// <returns>No content response</returns>
	/// <response code="204">If the category was deleted successfully</response>
	/// <response code="401">If user is not authenticated</response>
	/// <response code="403">If user does not have Admin role</response>
	/// <response code="404">If the category with the specified ID is not found</response>
	/// <response code="409">If the category cannot be deleted due to existing references</response>
	/// <response code="500">If an internal server error occurs</response>
	[HttpDelete("{id:int}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
	{
		await services.DeleteCategory(id, cancellationToken);
		return NoContent();
	}
}