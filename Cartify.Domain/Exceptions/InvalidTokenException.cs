namespace Cartify.Domain.Exceptions;

public class InvalidTokenException(string message) : ApplicationException(message)
{
}