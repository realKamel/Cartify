using Cartify.Shared.DataTransferObjects.User;

namespace Cartify.Services.Abstractions
{
	public interface IWishlistServices
	{
		Task<WishlistItemsResponse> GetWishlistItems(string? query, CancellationToken cancellationToken);
		Task RemoveItemFromWishlist(int id, CancellationToken cancellationToken);
		Task AddItemToWishlist(int id, CancellationToken cancellationToken);
	}
}
