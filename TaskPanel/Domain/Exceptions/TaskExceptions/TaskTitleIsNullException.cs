namespace Domain.Exceptions.TaskExceptions;

public class TaskTitleIsNullException : Exception
{
    public TaskTitleIsNullException(String ? message) : base(message)
    {
    }
}