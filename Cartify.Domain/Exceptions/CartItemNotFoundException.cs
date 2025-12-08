namespace Cartify.Domain.Exceptions;

public sealed class CartItemNotFoundException(string message) : NotFoundException(message)
{
}