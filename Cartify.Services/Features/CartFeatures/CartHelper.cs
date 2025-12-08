using Cartify.Domain.Entities;
using Cartify.Services.Features.ProductFeatures;
using Cartify.Shared.DataTransferObjects.User;

namespace Cartify.Services.Features.CartFeatures;

public static class CartHelper
{

	public static CartItemDto ToDto(this CartItem item, Product product)
	{
		return new CartItemDto
		{
			Id = item.Id,
			Product = product.ToProductResponseDto(),
			Count = item.Count,
		};
	}

	public static IList<CartItemDto> GenerateCartItemsFromProduct(IList<CartItem> cartItems, IDictionary<int, Product> products)
	{

		var items = new List<CartItemDto>(cartItems.Count);
		foreach (var item in cartItems)
		{
			if (products.TryGetValue(item.ProductId, out var product))
			{
				items.Add(item.ToDto(product));
			}
		}
		return items;
	}
}
