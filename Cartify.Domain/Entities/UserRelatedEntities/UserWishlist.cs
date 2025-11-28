
using Cartify.Domain.Entities.JoinEntities;

namespace Cartify.Domain.Entities.UserRelatedEntities
{
	public class UserWishlist : BaseEntity<int>
	{
		public required string UserId { get; set; }
		public ICollection<WishlistProduct> WishlistProducts { get; set; } = [];
	}
}
