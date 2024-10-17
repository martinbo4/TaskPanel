namespace Domain.Exceptions.TaskExceptions;

public class TaskDeadLineDateIsPreviousThanCurrentDate : Exception
{
    public TaskDeadLineDateIsPreviousThanCurrentDate(String ? message) : base(message)
    {
    }
}