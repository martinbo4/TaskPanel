namespace Domain.Exceptions.CommentExceptions;
public class CommentResolverIsNullException : Exception
{
    public CommentResolverIsNullException(string message) : base(message)
    {
    }
}