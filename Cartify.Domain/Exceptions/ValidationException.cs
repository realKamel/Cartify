namespace Cartify.Domain.Exceptions;

public class ValidationException(string message) : AppBaseException(message)
{
}