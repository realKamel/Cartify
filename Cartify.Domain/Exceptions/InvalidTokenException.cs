namespace Cartify.Domain.Exceptions;

public sealed class InvalidTokenException(string message) : ApplicationException(message)
{
}