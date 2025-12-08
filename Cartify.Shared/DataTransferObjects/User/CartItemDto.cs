using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Shared.DataTransferObjects.User
{
	public record CartItemDto
	{
		public required string Id { get; set; }
		public required int Count { get; set; }
		public decimal Price => Count * Product.Price;
		public required ProductResponseDto Product { get; set; }
	}
}
