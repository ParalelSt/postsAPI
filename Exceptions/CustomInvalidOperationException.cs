namespace PostsAPI.Exceptions;

public class CustomInvalidOperationException : Exception
{
    public CustomInvalidOperationException(string message) : base(message) { }
}