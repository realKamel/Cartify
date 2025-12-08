namespace Cartify.Domain.Exceptions;

public sealed class InvalidWishlistException(string message) : AppBaseException(message)
{
}
