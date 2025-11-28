using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartify.Domain.Entities.UserRelatedEntities;

namespace Cartify.Domain.Entities.JoinEntities
{
	public class WishlistProduct : BaseEntity<int>
	{
		public Product Product { get; set; }
		public int ProductId { get; set; }

		public UserWishlist UserWishlist { get; set; }
		public int UserWishlistId { get; set; }
	}
}
