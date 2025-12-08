namespace Cartify.Domain.Exceptions;

public sealed class UserAlreadyExistsException(string message) : ConflictException(message)
{
}
