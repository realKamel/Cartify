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
	internal class WishlistWithProductRelatedDataSpecification : Specification<Wishlist, int>
	{
		public WishlistWithProductRelatedDataSpecification(string userId) : base(w => w.UserId == userId)
		{
			AddRelatedDataInclude("Products");
			AddRelatedDataInclude("Products.Product");
			AddRelatedDataInclude("Products.Product.Brand");
			AddRelatedDataInclude("Products.Product.Category");
		}
	}
}
