namespace Cartify.Domain.Exceptions;

public class ConflictException(string message) : AppBaseException(message)
{
}