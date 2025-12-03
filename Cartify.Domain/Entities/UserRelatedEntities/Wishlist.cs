
using Cartify.Domain.Entities.JoinEntities;

namespace Cartify.Domain.Entities.UserRelatedEntities
{
	public class Wishlist : BaseEntity<int>
	{
		public required string UserId { get; set; }
		public IList<WishlistedProduct> Products { get; set; } = [];
	}
}
