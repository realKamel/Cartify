using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Presentation.Controllers;

[Authorize]
public class CartController(ICartServices services) : V1BaseController
{
	[HttpGet]
	[ProducesResponseType<CartResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<CartResponse>> GetCart(CancellationToken cancellationToken)
	{
		var result = await services.GetUserCartAsync(cancellationToken);
		return Ok(result);
	}

	[HttpPut("{productId}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> AddItemToCart(int productId, CancellationToken cancellationToken)
	{
		await services.AddItemAsync(productId, cancellationToken);
		return NoContent();
	}


	[HttpDelete("{itemId}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> RemoveItemFromCart(string itemId, CancellationToken cancellationToken)
	{
		await services.RemoveItemAsync(itemId, cancellationToken);
		return NoContent();
	}


	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]

	public async Task<ActionResult> RemoveCart(CancellationToken cancellationToken)
	{
		await services.ClearCartAsync(cancellationToken);
		return NoContent();
	}


	[HttpPatch]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult> UpdateCartItem(CartUpdateItemRequest request, CancellationToken cancellationToken)
	{
		await services.UpdateItemQuantityAsync(request.ItemId, request.NewCount, cancellationToken);
		return NoContent();
	}
}
