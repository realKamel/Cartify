using Cartify.Domain.Entities;
using Cartify.Services.Specifications;

namespace Cartify.Services.Features.CartFeatures
{
	public class ProductAsCartItemSpecification : Specification<Product, int>
	{
		public ProductAsCartItemSpecification(List<int> productIds)
			: base(p => productIds.Contains(p.Id))
		{
			AddRelatedDataInclude(p => p.Brand);
			AddRelatedDataInclude(p => p.Category);
		}
		public ProductAsCartItemSpecification(int productId)
			: base(p => p.Id == productId)
		{
			AddRelatedDataInclude(p => p.Brand);
			AddRelatedDataInclude(p => p.Category);
		}
	}
}
