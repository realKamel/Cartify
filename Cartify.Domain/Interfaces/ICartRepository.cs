using Cartify.Domain.Entities;

namespace Cartify.Domain.Interfaces
{
	public interface ICartRepository
	{
		// READ Operations
		public Task<Cart?> GetCartByUserIdAsync(string userId);

		// WRITE Operations
		// Bundle the "create/update" into one method:
		public Task<bool> CreateOrUpdateCartAsync(Cart cart, int expiryInDays = 7);
		// DELETE Operations
		Task<bool> RemoveCartAsync(string userId); // Removes the entire cart key
	}
}
