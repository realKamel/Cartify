using System.ComponentModel.DataAnnotations;
using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers
{
	/// <summary>
	/// Controller for managing user wishlist operations in the FreshCart e-commerce application.
	/// Provides endpoints for retrieving, adding, and removing wishlist items.
	/// </summary>
	/// <remarks>
	/// Inherits from V1BaseController and requires authenticated user context for all operations.
	/// </remarks>
	[Authorize]
	public class WishlistController(IWishlistServices services) : V1BaseController
	{
		/// <summary>
		/// Retrieves the current user's wishlist items.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
		/// <returns>
		/// <see cref="WishlistItemsResponse"/> containing the user's wishlist items
		/// </returns>
		/// <response code="200">Returns the user's wishlist items</response>
		/// <response code="401">If user is not authenticated</response>
		/// <response code="500">Internal server error</response>
		[HttpGet]
		[ProducesResponseType<WishlistItemsResponse>(StatusCodes.Status200OK)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<WishlistItemsResponse>> GetUserWishlist(CancellationToken cancellationToken)
		{
			var result = await services.GetWishlistItems(null, cancellationToken);
			return Ok(result);
		}


		/// <summary>
		/// Adds a product to the current user's wishlist.
		/// </summary>
		/// <param name="id">The product ID to add to wishlist</param>
		/// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
		/// <returns>
		/// <see cref="IActionResult"/> indicating the result of the operation
		/// </returns>
		/// <response code="201">Product successfully added to wishlist</response>
		/// <response code="400">Invalid product ID or other validation errors</response>
		/// <response code="401">If user is not authenticated</response>
		/// <response code="404">If the specified product does not exist</response>
		/// <response code="500">Internal server error</response>
		[HttpPost("{id:int}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> AddProductToWishlist([Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
		{
			await services.AddItemToWishlist(id, cancellationToken);
			return Created();
		}

		/// <summary>
		/// Removes a product from the current user's wishlist.
		/// </summary>
		/// <param name="id">The product ID to remove from wishlist</param>
		/// <param name="cancellationToken">Cancellation token for asynchronous operation</param>
		/// <returns>
		/// <see cref="IActionResult"/> indicating the result of the operation
		/// </returns>
		/// <response code="204">Product successfully removed from wishlist</response>
		/// <response code="400">Invalid product ID or other validation errors</response>
		/// <response code="401">If user is not authenticated</response>
		/// <response code="404">If the specified product is not in the user's wishlist</response>
		/// <response code="500">Internal server error</response>
		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
		[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> RemovedItemFromWishlist([Range(1, int.MaxValue)] int id, CancellationToken cancellationToken)
		{
			await services.RemoveItemFromWishlist(id, cancellationToken);
			return NoContent();
		}
	}
}
