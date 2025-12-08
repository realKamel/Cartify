using Cartify.Shared.DataTransferObjects.User;

namespace Cartify.Services.Abstractions
{
	public interface ICartServices
	{
		Task<CartResponse> GetUserCartAsync(CancellationToken cancellation = default);

		// WRITE/MODIFY OPERATIONS
		/// <summary>Adds a new product or increments the quantity of an existing one.</summary>
		Task AddItemAsync(int productId, CancellationToken cancellation = default);

		/// <summary>Updates the quantity of a specific cart item.</summary>
		Task UpdateItemQuantityAsync(string itemId, int newQuantity, CancellationToken cancellation = default);

		/// <summary>Removes a specific cart item.</summary>
		Task RemoveItemAsync(string itemId, CancellationToken cancellation = default);

		Task ClearCartAsync(CancellationToken cancellation = default);

		/// <summary>Converts the cart into a new order and clears the cart.</summary>
		//Task<OrderResponse> CheckoutAsync(string userId);
	}
}
