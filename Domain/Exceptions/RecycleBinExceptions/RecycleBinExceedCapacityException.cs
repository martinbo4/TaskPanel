namespace Domain.Exceptions.RecycleBinExceptions;

public class RecycleBinExceedCapacityException : Exception
{
    public RecycleBinExceedCapacityException(String ? message) : base(message)
    {
    }
}