namespace Cartify.Domain.Exceptions;

public sealed class CartNotFoundException(string message) : NotFoundException(message)
{
}
