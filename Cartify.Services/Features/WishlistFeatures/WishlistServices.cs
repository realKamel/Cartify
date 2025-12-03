using Cartify.Domain.Entities;
using Cartify.Domain.Entities.JoinEntities;
using Cartify.Domain.Entities.UserRelatedEntities;
using Cartify.Domain.Exceptions;
using Cartify.Domain.Interfaces;
using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects.User;

namespace Cartify.Services.Features.WishlistFeatures
{
	public class WishlistServices(IUnitOfWork unitOfWork, ICurrentHttpContext context) : IWishlistServices
	{
		private IGenericRepository<Wishlist, int> WishlistRepo => unitOfWork.GetOrCreateRepository<Wishlist, int>();
		private IGenericRepository<Product, int> ProductRepo => unitOfWork.GetOrCreateRepository<Product, int>();
		public async Task AddItemToWishlist(int productId, CancellationToken cancellationToken)
		{
			if (context.UserId is null)
			{
				throw new UnauthorizedAccessException("User is not authenticated.");
			}
			var foundProduct = await ProductRepo.ExistsAsync(productId, cancellationToken);

			if (!foundProduct)
			{
				throw new ProductNotFoundException();
			}

			var wishlist = await WishlistRepo.GetSingleAsync(new WishlistSpecification(context.UserId), cancellationToken);

			if (wishlist is null)
			{
				var list = new Wishlist()
				{
					UserId = context.UserId,
					Products = new List<WishlistedProduct>()
				};
				list.Products.Add(new WishlistedProduct() { ProductId = productId });
				WishlistRepo.Add(list);
			}
			else if (wishlist.Products.Any(wp => wp.ProductId == productId))
			{
				return;
			}
			else
			{
				wishlist.Products.Add(new WishlistedProduct() { ProductId = productId });
			}
			await unitOfWork.SaveChangesAsync(cancellationToken);
		}

		public async Task<WishlistItemsResponse> GetWishlistItems(string? query, CancellationToken cancellationToken)
		{
			if (context.UserId is null)
			{
				throw new UnauthorizedAccessException();
			}

			var wishlist = await WishlistRepo.GetSingleAsync(new WishlistWithProductRelatedDataSpecification(context.UserId), cancellationToken);

			if (wishlist is null)
			{
				throw new WishlistNotFoundException();
			}

			var result = wishlist.ToResponse();

			return result;
		}

		public async Task RemoveItemFromWishlist(int id, CancellationToken cancellationToken)
		{
			if (context.UserId is null)
			{
				throw new UnauthorizedAccessException();
			}
			var wishlist = await WishlistRepo.GetSingleAsync(new WishlistSpecification(context.UserId), cancellationToken);

			if (wishlist is null)
			{
				throw new WishlistNotFoundException();
			}

			var product = wishlist.Products.FirstOrDefault(p => p.ProductId == id) ?? throw new ProductNotFoundException();

			wishlist.Products.Remove(product);

			await unitOfWork.SaveChangesAsync(cancellationToken);
		}
	}
}
