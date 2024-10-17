namespace Domain.Exceptions.TaskExceptions;

public class TaskDescriptionIsNullException : Exception
{
    public TaskDescriptionIsNullException(String ? message) : base(message)
    {
    }
}