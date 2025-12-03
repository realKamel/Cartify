using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cartify.Domain.Entities.JoinEntities;
using Cartify.Domain.Entities.UserRelatedEntities;
using Cartify.Services.Specifications;

namespace Cartify.Services.Features.WishlistFeatures
{
	internal class WishlistProductSpecification : Specification<Wishlist, int>
	{
		public WishlistProductSpecification(string userId) : base(w => w.UserId == userId)
		{
			//AddRelatedDataInclude("Products");
			//AddRelatedDataInclude("Products.Product");
			AddRelatedDataInclude(p=>p.Products);
			AddRelatedDataInclude(p => p.Products.Select(x => x.Product));
		}
	}
}
