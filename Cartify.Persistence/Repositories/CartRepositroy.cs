using System.Text.Json;
using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using StackExchange.Redis;

namespace Cartify.Persistence.Repositories
{
	public class CartRepository(IConnectionMultiplexer connection) : ICartRepository
	{
		private readonly IDatabase _database = connection.GetDatabase();
		private static RedisKey GetCartKey(string userId) => $"cart:{userId}";
		public async Task<bool> CreateOrUpdateCartAsync(Cart cart, int expiryInDays = 7)
		{
			cart.UpdatedAtUtc = DateTimeOffset.UtcNow;
			var serializedCart = JsonSerializer.Serialize(cart);
			return await _database
				.StringSetAsync(GetCartKey(cart.Id), serializedCart, expiry: TimeSpan.FromDays(expiryInDays));
		}

		public async Task<Cart?> GetCartByUserIdAsync(string userId)
		{
			var result = await _database.StringGetAsync(GetCartKey(userId));
			if (result.HasValue)
			{
				return JsonSerializer.Deserialize<Cart>(result!);
			}
			return null;
		}

		public async Task<bool> RemoveCartAsync(string userId)
		{
			return await _database.KeyDeleteAsync(GetCartKey(userId));
		}
	}
}
