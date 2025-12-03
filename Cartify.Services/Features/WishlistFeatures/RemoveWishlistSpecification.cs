using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cartify.Domain.Entities.UserRelatedEntities;
using Cartify.Services.Specifications;

namespace Cartify.Services.Features.WishlistFeatures
{
	internal class RemoveWishlistSpecification : Specification<Wishlist, int>
	{
		public RemoveWishlistSpecification(string userId)
		: base(w => w.UserId == userId)
		{
			AddRelatedDataInclude("WishlistedProducts");
			AddRelatedDataInclude("WishlistedProducts.Product");
		}
	}
}
