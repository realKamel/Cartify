namespace Cartify.Domain.Exceptions;

public class DuplicateEmailException(string message) : AppBaseException(message)
{
}