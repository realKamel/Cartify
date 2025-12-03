using Cartify.Domain.Entities.UserRelatedEntities;

namespace Cartify.Domain.Entities.JoinEntities
{
	public class WishlistedProduct : BaseEntity<int>
	{
		public Product Product { get; set; }
		public int ProductId { get; set; }

		public Wishlist Wishlist { get; set; }
		public int WishlistId { get; set; }
	}
}
