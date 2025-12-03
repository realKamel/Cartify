using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartify.Shared.DataTransferObjects.Product;

namespace Cartify.Shared.DataTransferObjects.User
{
	public record WishlistProductDto
	{
		public ProductResponseDto Product { get; set; }
	}
}
