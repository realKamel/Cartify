using System.Collections.Frozen;
using Cartify.Domain.Entities;
using Cartify.Domain.Exceptions;
using Cartify.Domain.Interfaces;
using Cartify.Services.Abstractions;
using Cartify.Shared.DataTransferObjects.User;
using Microsoft.Extensions.Logging;

namespace Cartify.Services.Features.CartFeatures;

public class CartServices(ICartRepository repository, IUnitOfWork unitOfWork, ICurrentHttpContext context, ILogger<CartServices> logger) : ICartServices
{
	private readonly IGenericRepository<Product, int> ProductRepository = unitOfWork
		.GetOrCreateRepository<Product, int>();

	public async Task AddItemAsync(int productId, CancellationToken cancellation = default)
	{
		if (context.UserId is null)
		{
			throw new UnauthorizedAccessException("User is not Authorized");
		}

		var cart = await repository.GetCartByUserIdAsync(context.UserId);

		if (cart is null)
		{
			cart = new Cart
			{
				Id = context.UserId,
				CartItems = [],
			};
		}

		//var product = await ProductRepository.GetSingleAsync(new ProductAsCartItemSpecification(productId), cancellation);
		var isProductFound = await ProductRepository.ExistsAsync(productId, cancellation);

		if (!isProductFound)
		{
			throw new ProductNotFoundException();
		}

		var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

		if (existingItem is not null)
		{
			existingItem.Count++;
		}
		else
		{
			cart.CartItems.Add(new CartItem
			{
				Id = Guid.CreateVersion7().ToString(),
				ProductId = productId,
				Count = 1,
			});
		}
		await repository.CreateOrUpdateCartAsync(cart);
	}

	public async Task ClearCartAsync(CancellationToken cancellation = default)
	{
		if (context.UserId is null)
		{
			throw new UnauthorizedAccessException("User is not Authorized");
		}
		await repository.RemoveCartAsync(context.UserId);
	}

	public async Task<CartResponse> GetUserCartAsync(CancellationToken cancellation = default)
	{
		if (context.UserId is null)
		{
			throw new UnauthorizedAccessException("Login First");
		}

		var cart = await repository.GetCartByUserIdAsync(context.UserId);

		if (cart is null)
		{
			throw new CartNotFoundException("Cart not found");
		}

		// Here We must extract the product ids in cart and retrieve them from database with latest state
		// As a inner join operation
		//Perform the query using specification

		var products = await ProductRepository
			.GetAllAsync(new ProductAsCartItemSpecification([.. cart.CartItems.Select(cartItem => cartItem.ProductId)]), cancellationToken: cancellation);

		var cartItems = CartHelper
			.GenerateCartItemsFromProduct(cart.CartItems, products.ToFrozenDictionary(p => p.Id));

		return new CartResponse
		{
			Id = cart.Id,
			CartItems = cartItems,
		};
	}

	public async Task RemoveItemAsync(string itemId, CancellationToken cancellation = default)
	{
		if (context.UserId is null)
		{
			throw new UnauthorizedAccessException("Login First");
		}

		var cart = await repository.GetCartByUserIdAsync(context.UserId);

		if (cart is null)
		{
			throw new CartNotFoundException("Cart not Found");
		}

		var element = cart.CartItems.FirstOrDefault(i => i.Id == itemId);

		if (element is null)
		{
			throw new CartItemNotFoundException("Product not Found");
		}

		if (cart.CartItems.Remove(element))
		{
			await repository.CreateOrUpdateCartAsync(cart);
		}
		else
		{
			logger.LogError("Error Happened while removing item from cart for User with id:{userId}", context.UserId);
			throw new Exception("Internal Server Error in removing item from user Cart");
		}
	}

	public async Task UpdateItemQuantityAsync(string itemId, int newCount, CancellationToken cancellation = default)
	{
		if (context.UserId is null)
		{
			throw new UnauthorizedAccessException("Login First");
		}

		var cart = await repository.GetCartByUserIdAsync(context.UserId);

		if (cart is null)
		{
			throw new CartNotFoundException("Cart not Found");
		}

		var element = cart.CartItems.FirstOrDefault(i => i.Id == itemId);

		if (element is null)
		{
			throw new CartItemNotFoundException("Cart Item not Found");
		}

		element.Count = newCount;
		element.UpdatedAtUtc = DateTimeOffset.UtcNow;

		//cart.CartItems[cart.CartItems.IndexOf(element)] = element;

		await repository.CreateOrUpdateCartAsync(cart);
	}
}
