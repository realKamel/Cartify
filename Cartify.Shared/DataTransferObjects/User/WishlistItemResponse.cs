using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Shared.DataTransferObjects.User
{
	public record WishlistItemsResponse
	{
		public ICollection<ProductResponseDto> Products { get; set; }
		public int Count => Products.Count;
	}
}
