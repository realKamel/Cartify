using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Domain.Exceptions
{
	public class InvalidWishlistException(string message) : AppBaseException(message)
	{
	}
}
