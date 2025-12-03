using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartify.Domain.Entities.JoinEntities;
using Cartify.Domain.Entities.UserRelatedEntities;
using Cartify.Services.Features.ProductFeatures;
using Cartify.Shared.DataTransferObjects.User;

namespace Cartify.Services.Features.WishlistFeatures
{
	static class WishlistHelper
	{
		public static WishlistItemsResponse ToResponse(this Wishlist wishlist)
		{
			return new WishlistItemsResponse
			{
				Products = wishlist.Products.Select(x => x.Product.ToProductResponseDto()).ToList(),
			};
		}

		public static WishlistProductDto ToDto(this WishlistedProduct product)
		{
			return new WishlistProductDto
			{
				Product = product.Product.ToProductResponseDto()
			};
		}
	}
}
