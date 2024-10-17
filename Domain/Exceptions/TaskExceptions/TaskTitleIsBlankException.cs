namespace Exceptions;

public class TaskTitleIsBlankException: Exception
{
    public TaskTitleIsBlankException(String message): base(message)
    {
    }
}