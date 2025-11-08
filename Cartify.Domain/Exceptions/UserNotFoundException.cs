namespace Cartify.Domain.Exceptions;

public class UserNotFoundException(string message) : NotFoundException(message)
{
}